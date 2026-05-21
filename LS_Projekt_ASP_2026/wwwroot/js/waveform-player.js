document.addEventListener('DOMContentLoaded', () => {
    // ===== DOM Elements =====
    const uploadArea = document.getElementById('uploadArea');
    const audioFileInput = document.getElementById('audioFileInput');
    const uploadSection = document.getElementById('uploadSection');
    const playerSection = document.getElementById('playerSection');
    
    const audioSource = document.getElementById('audioSource');
    const playBtn = document.getElementById('playBtn');
    const currentTimeEl = document.getElementById('currentTime');
    const durationEl = document.getElementById('duration');
    const timelineContainer = document.getElementById('timelineContainer');
    const timelineCanvas = document.getElementById('timelineCanvas');
    const playhead = document.getElementById('playhead');
    const loadingOverlay = document.getElementById('loadingOverlay');
    const visualizerCanvas = document.getElementById('visualizerCanvas');
    
    const trackTitle = document.getElementById('trackTitle');
    const trackDuration = document.getElementById('trackDuration');
    
    // Comments
    const addCommentBtn = document.getElementById('addCommentBtn');
    const addCommentForm = document.getElementById('addCommentForm');
    const commentText = document.getElementById('commentText');
    const commentTime = document.getElementById('commentTime');
    const commentTimeInput = document.getElementById('commentTimeInput');
    const saveCommentBtn = document.getElementById('saveCommentBtn');
    const cancelCommentBtn = document.getElementById('cancelCommentBtn');
    const commentsList = document.getElementById('commentsList');

    // ===== State =====
    let audioCtx;
    let analyser;
    let sourceNode;
    let isInitialized = false;
    
    let audioBuffer = null;
    let peaks = [];
    let isDragging = false;
    let comments = []; // Array to store comments with timestamp
    let pendingCommentTime = null;
    let currentStorageKey = null;
    let activeCommentId = null;
    let currentAudioUrl = null;
    const initialPlayerData = window.playerInitialData || null;
    const serverMode = Boolean(initialPlayerData?.projectVersionId);

    // ===== File Upload Handling =====
    uploadArea.addEventListener('click', () => {
        audioFileInput.click();
    });

    audioFileInput.addEventListener('change', (e) => {
        const file = e.target.files[0];
        if (file) {
            handleAudioFile(file);
        }
    });

    // Drag & Drop
    uploadArea.addEventListener('dragover', (e) => {
        e.preventDefault();
        uploadArea.classList.add('drag-over');
    });

    uploadArea.addEventListener('dragleave', () => {
        uploadArea.classList.remove('drag-over');
    });

    uploadArea.addEventListener('drop', (e) => {
        e.preventDefault();
        uploadArea.classList.remove('drag-over');
        const file = e.dataTransfer.files[0];
        if (file && file.type.startsWith('audio/')) {
            handleAudioFile(file);
        }
    });

    function handleAudioFile(file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            const arrayBuffer = e.target.result;
            loadAudioBuffer(arrayBuffer, file);
        };
        reader.readAsArrayBuffer(file);
    }

    async function loadAudioBuffer(arrayBuffer, file) {
        try {
            loadingOverlay.style.display = 'flex';
            
            const offlineCtx = new (window.AudioContext || window.webkitAudioContext)();
            audioBuffer = await offlineCtx.decodeAudioData(arrayBuffer);
            
            if (currentAudioUrl) {
                URL.revokeObjectURL(currentAudioUrl);
            }

            currentAudioUrl = URL.createObjectURL(file);
            audioSource.pause();
            audioSource.src = currentAudioUrl;
            audioSource.load();
            audioSource.currentTime = 0;
            playBtn.classList.remove('playing');
            playhead.style.left = '0%';
            currentTimeEl.textContent = '0:00';
            
            // Update UI
            const fileName = file.name;
            trackTitle.textContent = fileName.replace(/\.[^.]+$/, '');
            trackDuration.textContent = formatTime(audioBuffer.duration);
            durationEl.textContent = formatTime(audioBuffer.duration);
            currentStorageKey = getStorageKey(fileName, audioBuffer.duration);
            
            // Extract and draw waveform
            extractPeaks(audioBuffer);
            drawStaticWaveform();
            
            // Load comments saved for this audio file.
            activeCommentId = null;
            comments = loadCommentsFromStorage();
            renderComments();
            
            // Show player, hide upload
            uploadSection.style.display = 'none';
            playerSection.style.display = 'block';
            playBtn.disabled = false;
            
            loadingOverlay.style.display = 'none';
        } catch (err) {
            console.error('Error loading audio:', err);
            loadingOverlay.innerHTML = '<span>Error loading audio file. Please try another file.</span>';
        }
    }

    function extractPeaks(buffer) {
        const channelData = buffer.getChannelData(0);
        const samples = 150;
        const blockSize = Math.floor(channelData.length / samples);
        
        peaks = [];
        for (let i = 0; i < samples; i++) {
            let start = blockSize * i;
            let sum = 0;
            for (let j = 0; j < blockSize; j++) {
                sum += Math.abs(channelData[start + j]);
            }
            peaks.push(sum / blockSize);
        }

        const max = Math.max(...peaks);
        peaks = peaks.map(p => p / max);
    }

    function drawStaticWaveform() {
        const dpr = window.devicePixelRatio || 1;
        const rect = timelineContainer.getBoundingClientRect();
        
        timelineCanvas.width = rect.width * dpr;
        timelineCanvas.height = rect.height * dpr;
        
        const ctx = timelineCanvas.getContext('2d');
        ctx.scale(dpr, dpr);
        ctx.clearRect(0, 0, rect.width, rect.height);
        
        const barWidth = rect.width / peaks.length;
        const gap = 2;
        
        const grad = ctx.createLinearGradient(0, 0, 0, rect.height);
        grad.addColorStop(0, 'rgba(124, 58, 237, 0.8)');
        grad.addColorStop(1, 'rgba(6, 182, 212, 0.8)');
        
        ctx.fillStyle = grad;
        
        for (let i = 0; i < peaks.length; i++) {
            const val = peaks[i];
            const h = Math.max(2, val * (rect.height * 0.8));
            const y = (rect.height - h) / 2;
            
            ctx.beginPath();
            ctx.roundRect(i * barWidth, y, barWidth - gap, h, 2);
            ctx.fill();
        }
    }

    // ===== Web Audio API Setup =====
    function setupWebAudio() {
        if (isInitialized) return;
        
        audioCtx = new (window.AudioContext || window.webkitAudioContext)();
        analyser = audioCtx.createAnalyser();
        analyser.fftSize = 256;
        
        sourceNode = audioCtx.createMediaElementSource(audioSource);
        sourceNode.connect(analyser);
        analyser.connect(audioCtx.destination);
        
        isInitialized = true;
        renderVisualizer();
    }

    function renderVisualizer() {
        const dpr = window.devicePixelRatio || 1;
        const rect = visualizerCanvas.parentElement.getBoundingClientRect();
        
        visualizerCanvas.width = rect.width * dpr;
        visualizerCanvas.height = 80 * dpr;
        
        const ctx = visualizerCanvas.getContext('2d');
        ctx.scale(dpr, dpr);
        
        const bufferLength = analyser.frequencyBinCount;
        const dataArray = new Uint8Array(bufferLength);
        
        function draw() {
            requestAnimationFrame(draw);
            
            const w = rect.width;
            const h = 80;
            
            analyser.getByteFrequencyData(dataArray);
            
            ctx.clearRect(0, 0, w, h);
            
            const barWidth = (w / bufferLength) * 2.5;
            let x = 0;
            
            for(let i = 0; i < bufferLength; i++) {
                const barHeight = (dataArray[i] / 255) * h;
                
                const r = 6 + (barHeight * 2);
                const g = 182 - barHeight;
                const b = 212;
                
                ctx.fillStyle = `rgb(${r}, ${g}, ${b})`;
                ctx.fillRect(x, h - barHeight, barWidth, barHeight);
                
                x += barWidth + 1;
            }
        }
        
        draw();
    }

    // ===== Time Utilities =====
    function formatTime(secs) {
        if (isNaN(secs)) return "0:00";
        const minutes = Math.floor(secs / 60);
        const seconds = Math.floor(secs % 60);
        return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
    }

    function parseTimeInput(value) {
        const cleanValue = value.trim();
        if (!cleanValue) return NaN;

        if (!cleanValue.includes(':')) {
            return parseFloat(cleanValue.replace(',', '.'));
        }

        const parts = cleanValue.split(':').map(part => parseFloat(part));
        if (parts.some(part => isNaN(part))) return NaN;

        return parts.reduce((total, part) => (total * 60) + part, 0);
    }

    function clampTime(time) {
        const duration = audioSource.duration || audioBuffer?.duration || 0;
        if (isNaN(time)) return NaN;
        return Math.min(Math.max(time, 0), duration || time);
    }

    function updatePlayhead() {
        if (!audioSource.duration) return;
        const percent = audioSource.currentTime / audioSource.duration;
        playhead.style.left = `${percent * 100}%`;
        currentTimeEl.textContent = formatTime(audioSource.currentTime);
        
        if (!isDragging) {
            requestAnimationFrame(updatePlayhead);
        }
    }

    // ===== Playback Controls =====
    playBtn.addEventListener('click', async () => {
        if (!audioSource.src) return;

        setupWebAudio();
        
        if (audioCtx.state === 'suspended') {
            await audioCtx.resume();
        }
        
        if (audioSource.paused) {
            try {
                await audioSource.play();
                playBtn.classList.add('playing');
                requestAnimationFrame(updatePlayhead);
            } catch (err) {
                console.error('Unable to play audio:', err);
                playBtn.classList.remove('playing');
            }
        } else {
            audioSource.pause();
            playBtn.classList.remove('playing');
        }
    });

    audioSource.addEventListener('play', () => {
        playBtn.classList.add('playing');
        requestAnimationFrame(updatePlayhead);
    });

    audioSource.addEventListener('pause', () => {
        playBtn.classList.remove('playing');
    });

    audioSource.addEventListener('loadedmetadata', () => {
        durationEl.textContent = formatTime(audioSource.duration);
    });

    audioSource.addEventListener('ended', () => {
        playBtn.classList.remove('playing');
        playhead.style.left = '0%';
        currentTimeEl.textContent = "0:00";
    });

    // ===== Timeline Seeking & Comments =====
    function seekTo(event) {
        if (!audioBuffer) return;
        const rect = timelineContainer.getBoundingClientRect();
        const x = Math.max(0, Math.min(event.clientX - rect.left, rect.width));
        const percent = x / rect.width;
        playhead.style.left = `${percent * 100}%`;
        
        if (audioSource.duration) {
            audioSource.currentTime = percent * audioSource.duration;
            currentTimeEl.textContent = formatTime(audioSource.currentTime);
        }
    }

    async function loadServerAudio(versionData) {
        try {
            loadingOverlay.style.display = 'flex';
            uploadSection.style.display = 'none';
            playerSection.style.display = 'block';

            audioSource.pause();
            audioSource.src = versionData.audioUrl;
            audioSource.load();
            audioSource.currentTime = 0;
            playBtn.classList.remove('playing');
            playhead.style.left = '0%';
            currentTimeEl.textContent = '0:00';

            trackTitle.textContent = versionData.title || 'Audio verzija';
            trackDuration.textContent = versionData.durationSeconds ? formatTime(versionData.durationSeconds) : '--:--';
            durationEl.textContent = versionData.durationSeconds ? formatTime(versionData.durationSeconds) : '0:00';

            const response = await fetch(versionData.audioUrl);
            const arrayBuffer = await response.arrayBuffer();
            const offlineCtx = new (window.AudioContext || window.webkitAudioContext)();
            audioBuffer = await offlineCtx.decodeAudioData(arrayBuffer);

            trackDuration.textContent = formatTime(audioBuffer.duration);
            durationEl.textContent = formatTime(audioBuffer.duration);

            extractPeaks(audioBuffer);
            drawStaticWaveform();

            comments = (versionData.comments || []).map(comment => ({
                id: String(comment.id),
                time: parseFloat(comment.time),
                text: comment.text,
                author: comment.author,
                timestamp: comment.timestamp
            }));
            activeCommentId = null;
            renderComments();

            playBtn.disabled = false;
            loadingOverlay.style.display = 'none';
        } catch (err) {
            console.error('Error loading server audio:', err);
            loadingOverlay.innerHTML = '<span>Error loading audio file. Please try another file.</span>';
        }
    }

    function getTimelineTime(event) {
        const rect = timelineContainer.getBoundingClientRect();
        const x = Math.max(0, Math.min(event.clientX - rect.left, rect.width));
        const percent = x / rect.width;
        return percent * (audioSource.duration || audioBuffer?.duration || 0);
    }

    timelineContainer.addEventListener('mousedown', (e) => {
        isDragging = true;
        seekTo(e);
    });

    document.addEventListener('mousemove', (e) => {
        if (isDragging) {
            seekTo(e);
        }
    });

    document.addEventListener('mouseup', () => {
        if (isDragging) {
            isDragging = false;
            if (!audioSource.paused) {
                requestAnimationFrame(updatePlayhead);
            }
        }
    });

    // Double-click the timeline to add a comment at that exact moment.
    timelineContainer.addEventListener('dblclick', (e) => {
        if (!audioBuffer || e.target.closest('.comment-marker')) return;
        const time = getTimelineTime(e);
        audioSource.currentTime = time;
        currentTimeEl.textContent = formatTime(time);
        playhead.style.left = `${(time / audioSource.duration) * 100}%`;
        openCommentForm(time);
    });

    // ===== Comments System =====
    function openCommentForm(time) {
        if (isNaN(time) || time === null || time === undefined) {
            time = 0;
        }
        pendingCommentTime = clampTime(time);
        commentTime.textContent = formatTime(pendingCommentTime);
        commentTimeInput.value = formatTime(pendingCommentTime);
        commentText.value = '';
        addCommentForm.style.display = 'block';
        commentText.focus();
    }

    addCommentBtn.addEventListener('click', () => {
        if (!audioSource || isNaN(audioSource.currentTime)) {
            openCommentForm(0);
        } else {
            openCommentForm(audioSource.currentTime);
        }
    });

    commentTimeInput.addEventListener('blur', () => {
        const parsedTime = clampTime(parseTimeInput(commentTimeInput.value));
        if (isNaN(parsedTime)) {
            commentTimeInput.value = formatTime(pendingCommentTime || 0);
            return;
        }

        pendingCommentTime = parsedTime;
        commentTimeInput.value = formatTime(parsedTime);
        commentTime.textContent = formatTime(parsedTime);
    });

    saveCommentBtn.addEventListener('click', async () => {
        const text = commentText.value.trim();
        const adjustedTime = clampTime(parseTimeInput(commentTimeInput.value));
        if (!text || isNaN(adjustedTime)) return;
        
        let newComment;

        if (serverMode) {
            newComment = await saveCommentToServer(adjustedTime, text);
            if (!newComment) return;
        } else {
            newComment = {
                id: window.crypto?.randomUUID ? window.crypto.randomUUID() : `${Date.now()}-${Math.random()}`,
                time: adjustedTime,
                text: text,
                author: 'You',
                timestamp: new Date().toLocaleString()
            };
        }
        
        comments.push(newComment);
        
        // Sort comments by time
        comments.sort((a, b) => a.time - b.time);
        
        if (!serverMode) {
            saveCommentsToStorage();
        }
        
        addCommentForm.style.display = 'none';
        commentText.value = '';
        pendingCommentTime = null;
        jumpToComment(newComment.time, newComment.id);
    });

    async function saveCommentToServer(time, text) {
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        try {
            const response = await fetch('/Player?handler=Comment', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token || ''
                },
                body: JSON.stringify({
                    projectVersionId: initialPlayerData.projectVersionId,
                    timestampSeconds: time,
                    message: text
                })
            });

            if (!response.ok) {
                throw new Error(`Server returned ${response.status}`);
            }

            const savedComment = await response.json();
            return {
                id: String(savedComment.id),
                time: parseFloat(savedComment.time),
                text: savedComment.text,
                author: savedComment.author,
                timestamp: savedComment.timestamp
            };
        } catch (err) {
            console.error('Error saving comment:', err);
            return null;
        }
    }

    cancelCommentBtn.addEventListener('click', () => {
        addCommentForm.style.display = 'none';
        commentText.value = '';
        pendingCommentTime = null;
    });

    function renderComments() {
        renderCommentMarkers();

        if (comments.length === 0) {
            commentsList.innerHTML = '<p class="text-muted text-center py-4">Još nema komentara. Pomakni playhead pa dodaj komentar ili dvoklikni timeline.</p>';
            return;
        }

        commentsList.innerHTML = comments.map((comment, index) => {
            const time = parseFloat(comment.time);
            const isActive = comment.id === activeCommentId ? ' active' : '';
            const deleteButton = serverMode
                ? ''
                : `<button class="btn btn-sm btn-danger" onclick="deleteComment(${index})" style="align-self: flex-start;" aria-label="Delete comment">&times;</button>`;
            return `
            <div class="comment-item${isActive}" data-comment-id="${comment.id}">
                <button class="comment-timestamp" type="button" data-seek-time="${time}" aria-label="Jump to ${formatTime(time)}">${formatTime(time)}</button>
                <div class="comment-content">
                    <p>${escapeHtml(comment.text)}</p>
                    <p class="comment-author">${comment.author}</p>
                </div>
                ${deleteButton}
            </div>
            `;
        }).join('');
    }

    function renderCommentMarkers() {
        timelineContainer.querySelectorAll('.comment-marker').forEach(marker => marker.remove());

        if (!audioSource.duration || comments.length === 0) return;

        comments.forEach((comment) => {
            const marker = document.createElement('button');
            marker.type = 'button';
            marker.className = `comment-marker${comment.id === activeCommentId ? ' active' : ''}`;
            marker.style.left = `${Math.min(100, Math.max(0, (comment.time / audioSource.duration) * 100))}%`;
            marker.dataset.commentId = comment.id;
            marker.dataset.seekTime = comment.time;
            marker.setAttribute('aria-label', `Comment at ${formatTime(comment.time)}`);
            marker.innerHTML = `
                <span class="comment-marker-dot"></span>
                <span class="comment-popover">
                    <strong>${formatTime(comment.time)}</strong>
                    <span>${escapeHtml(comment.text)}</span>
                </span>
            `;
            timelineContainer.appendChild(marker);
        });
    }

    commentsList.addEventListener('click', (event) => {
        const seekButton = event.target.closest('[data-seek-time]');
        if (!seekButton) return;

        jumpToComment(seekButton.dataset.seekTime, seekButton.closest('[data-comment-id]')?.dataset.commentId);
    });

    timelineContainer.addEventListener('click', (event) => {
        const marker = event.target.closest('.comment-marker');
        if (!marker) return;

        event.stopPropagation();
        jumpToComment(marker.dataset.seekTime, marker.dataset.commentId);
    });

    function jumpToComment(time, commentId) {
        const parsedTime = parseFloat(time);
        if (isNaN(parsedTime) || !audioSource.duration) return;

        audioSource.currentTime = parsedTime;
        currentTimeEl.textContent = formatTime(parsedTime);
        playhead.style.left = `${(parsedTime / audioSource.duration) * 100}%`;
        activeCommentId = commentId || null;
        renderComments();

        const activeItem = commentsList.querySelector(`[data-comment-id="${activeCommentId}"]`);
        activeItem?.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }

    window.deleteComment = function(index) {
        comments.splice(index, 1);
        activeCommentId = null;
        saveCommentsToStorage();
        renderComments();
    };

    function escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    // ===== LocalStorage Persistence =====
    const STORAGE_PREFIX = 'waveform-player-comments';

    function getStorageKey(fileName, duration) {
        const normalizedName = fileName.trim().toLowerCase();
        const roundedDuration = Math.round(duration || 0);
        return `${STORAGE_PREFIX}:${normalizedName}:${roundedDuration}`;
    }

    function saveCommentsToStorage() {
        if (!currentStorageKey) return;

        try {
            localStorage.setItem(currentStorageKey, JSON.stringify(comments));
        } catch (err) {
            console.error('Error saving comments to localStorage:', err);
        }
    }

    function loadCommentsFromStorage() {
        try {
            if (!currentStorageKey) return [];

            const stored = localStorage.getItem(currentStorageKey);
            if (stored) {
                const parsed = JSON.parse(stored);
                if (Array.isArray(parsed)) {
                    return parsed.map(c => ({
                        id: c.id || `${Date.now()}-${Math.random()}`,
                        time: parseFloat(c.time),
                        text: c.text,
                        author: c.author,
                        timestamp: c.timestamp
                    }))
                        .filter(c => !isNaN(c.time) && c.text)
                        .sort((a, b) => a.time - b.time);
                }
            }
        } catch (err) {
            console.error('Error loading comments from localStorage:', err);
        }

        return [];
    }

    // ===== Resize Handling =====
    window.addEventListener('resize', () => {
        if (peaks.length > 0) {
            drawStaticWaveform();
            renderCommentMarkers();
        }
    });

    if (serverMode) {
        loadServerAudio(initialPlayerData);
    } else if (audioSource.src) {
        loadAudioData();
    }

    async function loadAudioData() {
        try {
            const response = await fetch(audioSource.src);
            const arrayBuffer = await response.arrayBuffer();
            
            const offlineCtx = new (window.AudioContext || window.webkitAudioContext)();
            audioBuffer = await offlineCtx.decodeAudioData(arrayBuffer);
            
            extractPeaks(audioBuffer);
            drawStaticWaveform();
            
            loadingOverlay.style.display = 'none';
            playBtn.disabled = false;
        } catch (err) {
            console.error('Error loading audio:', err);
        }
    }

    // Comments are loaded after an audio file is selected because storage is scoped per file.
});
