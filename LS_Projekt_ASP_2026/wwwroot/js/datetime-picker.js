document.addEventListener('DOMContentLoaded', () => {
  document.querySelectorAll('[data-datetime-picker]').forEach((element) => {
    initDateTimePicker(element);
  });
});

function initDateTimePicker(root) {
  const hiddenInput = root.querySelector('.datetime-hidden');
  const displayInput = root.querySelector('.datetime-display');
  const toggleButton = root.querySelector('.datetime-toggle');
  const panel = root.querySelector('.datetime-panel');
  const monthLabel = root.querySelector('.datetime-month-label');
  const weekdaysWrap = root.querySelector('.datetime-weekdays');
  const calendarWrap = root.querySelector('.datetime-calendar');
  const prevButton = root.querySelector('.datetime-nav--prev');
  const nextButton = root.querySelector('.datetime-nav--next');
  const hourSelect = root.querySelector('.datetime-hour');
  const minuteSelect = root.querySelector('.datetime-minute');
  const clearButton = root.querySelector('.datetime-clear');
  const cancelButton = root.querySelector('.datetime-cancel');
  const applyButton = root.querySelector('.datetime-apply');
  const showTime = root.dataset.showTime === 'true';
  const locale = document.documentElement.lang || navigator.language || 'hr-HR';

  if (!hiddenInput || !displayInput || !toggleButton || !panel || !monthLabel || !weekdaysWrap || !calendarWrap) {
    return;
  }

  const weekdayFormatter = new Intl.DateTimeFormat(locale, { weekday: 'short' });
  const monthFormatter = new Intl.DateTimeFormat(locale, { month: 'long', year: 'numeric' });
  const dateFormatter = new Intl.DateTimeFormat(locale, { day: '2-digit', month: '2-digit', year: 'numeric' });
  const dateTimeFormatter = new Intl.DateTimeFormat(locale, {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });

  const weekdayNames = Array.from({ length: 7 }, (_, index) => weekdayFormatter.format(new Date(2024, 0, 1 + index)));
  const minuteStep = 1;

  let selectedDate = parseStoredValue(hiddenInput.value, showTime);
  let draftDate = selectedDate ? new Date(selectedDate.getTime()) : new Date();
  let viewDate = new Date(draftDate.getFullYear(), draftDate.getMonth(), 1);
  let isOpen = false;

  displayInput.readOnly = true;
  displayInput.setAttribute('aria-haspopup', 'dialog');
  panel.setAttribute('aria-hidden', 'true');

  if (hourSelect && showTime) {
    hourSelect.innerHTML = '';
    for (let hour = 0; hour < 24; hour += 1) {
      const option = document.createElement('option');
      option.value = String(hour);
      option.textContent = String(hour).padStart(2, '0');
      hourSelect.appendChild(option);
    }
  }

  if (minuteSelect && showTime) {
    minuteSelect.innerHTML = '';
    for (let minute = 0; minute < 60; minute += minuteStep) {
      const option = document.createElement('option');
      option.value = String(minute);
      option.textContent = String(minute).padStart(2, '0');
      minuteSelect.appendChild(option);
    }
  }

  renderWeekdays();
  syncControlsFromDate(draftDate);
  render();
  updateDisplay();

  toggleButton.addEventListener('click', (event) => {
    event.preventDefault();
    if (isOpen) {
      closePanel();
      return;
    }

    openPanel();
  });

  displayInput.addEventListener('click', (event) => {
    event.preventDefault();
    if (!isOpen) {
      openPanel();
    }
  });

  if (prevButton) {
    prevButton.addEventListener('click', () => {
      viewDate = new Date(viewDate.getFullYear(), viewDate.getMonth() - 1, 1);
      render();
    });
  }

  if (nextButton) {
    nextButton.addEventListener('click', () => {
      viewDate = new Date(viewDate.getFullYear(), viewDate.getMonth() + 1, 1);
      render();
    });
  }

  if (hourSelect) {
    hourSelect.addEventListener('change', () => {
      draftDate.setHours(parseInt(hourSelect.value, 10));
      syncHiddenPreview();
    });
  }

  if (minuteSelect) {
    minuteSelect.addEventListener('change', () => {
      draftDate.setMinutes(parseInt(minuteSelect.value, 10));
      syncHiddenPreview();
    });
  }

  if (clearButton) {
    clearButton.addEventListener('click', () => {
      selectedDate = null;
      draftDate = new Date();
      hiddenInput.value = '';
      displayInput.value = '';
      closePanel();
      render();
    });
  }

  if (cancelButton) {
    cancelButton.addEventListener('click', () => {
      draftDate = selectedDate ? new Date(selectedDate.getTime()) : new Date();
      viewDate = new Date(draftDate.getFullYear(), draftDate.getMonth(), 1);
      syncControlsFromDate(draftDate);
      render();
      closePanel();
    });
  }

  if (applyButton) {
    applyButton.addEventListener('click', () => {
      commitDraft();
      closePanel();
    });
  }

  document.addEventListener('mousedown', handleOutsideClick, true);
  document.addEventListener('keydown', handleEscapeKey);

  function openPanel() {
    isOpen = true;
    root.classList.add('is-open');
    panel.classList.remove('d-none');
    panel.setAttribute('aria-hidden', 'false');
    viewDate = selectedDate ? new Date(selectedDate.getFullYear(), selectedDate.getMonth(), 1) : new Date(draftDate.getFullYear(), draftDate.getMonth(), 1);
    draftDate = selectedDate ? new Date(selectedDate.getTime()) : new Date(draftDate.getTime());
    syncControlsFromDate(draftDate);
    render();
    window.requestAnimationFrame(() => {
      const focusTarget = showTime && hourSelect ? hourSelect : calendarWrap.querySelector('.datetime-day');
      if (focusTarget) {
        focusTarget.focus();
      }
    });
  }

  function closePanel() {
    isOpen = false;
    root.classList.remove('is-open');
    panel.classList.add('d-none');
    panel.setAttribute('aria-hidden', 'true');
  }

  function handleOutsideClick(event) {
    if (!isOpen || root.contains(event.target)) {
      return;
    }

    closePanel();
  }

  function handleEscapeKey(event) {
    if (event.key === 'Escape' && isOpen) {
      closePanel();
    }
  }

  function renderWeekdays() {
    weekdaysWrap.innerHTML = '';
    weekdayNames.forEach((weekday) => {
      const label = document.createElement('span');
      label.textContent = weekday;
      weekdaysWrap.appendChild(label);
    });
  }

  function render() {
    monthLabel.textContent = monthFormatter.format(viewDate);
    calendarWrap.innerHTML = '';

    const firstDay = new Date(viewDate.getFullYear(), viewDate.getMonth(), 1);
    const daysInMonth = new Date(viewDate.getFullYear(), viewDate.getMonth() + 1, 0).getDate();
    const leadingEmptyDays = (firstDay.getDay() + 6) % 7;

    for (let index = 0; index < leadingEmptyDays; index += 1) {
      const placeholder = document.createElement('span');
      placeholder.setAttribute('aria-hidden', 'true');
      calendarWrap.appendChild(placeholder);
    }

    for (let day = 1; day <= daysInMonth; day += 1) {
      const date = new Date(viewDate.getFullYear(), viewDate.getMonth(), day);
      const button = document.createElement('button');
      button.type = 'button';
      button.className = 'datetime-day';
      button.textContent = String(day);
      button.setAttribute('aria-label', dateFormatter.format(date));

      if (selectedDate && isSameDay(date, selectedDate)) {
        button.classList.add('is-selected');
      }

      if (isSameDay(date, new Date())) {
        button.setAttribute('aria-current', 'date');
      }

      button.addEventListener('click', () => {
        setDraftDate(date);
        if (!showTime) {
          commitDraft();
          closePanel();
        }
      });

      calendarWrap.appendChild(button);
    }
  }

  function setDraftDate(date) {
    draftDate = new Date(date.getTime());

    if (showTime) {
      if (selectedDate) {
        draftDate.setHours(selectedDate.getHours(), selectedDate.getMinutes(), 0, 0);
      } else if (hourSelect && minuteSelect) {
        draftDate.setHours(parseInt(hourSelect.value, 10), parseInt(minuteSelect.value, 10), 0, 0);
      }
      syncControlsFromDate(draftDate);
    }

    viewDate = new Date(draftDate.getFullYear(), draftDate.getMonth(), 1);
    render();
    syncHiddenPreview();
  }

  function syncControlsFromDate(date) {
    if (!showTime) {
      return;
    }

    if (hourSelect) {
      hourSelect.value = String(date.getHours());
    }

    if (minuteSelect) {
      minuteSelect.value = String(date.getMinutes());
    }
  }

  function syncHiddenPreview() {
    if (!selectedDate && !isOpen) {
      return;
    }

    if (showTime) {
      displayInput.value = dateTimeFormatter.format(draftDate);
      return;
    }

    displayInput.value = dateFormatter.format(draftDate);
  }

  function commitDraft() {
    selectedDate = new Date(draftDate.getTime());

    if (showTime) {
      selectedDate.setSeconds(0, 0);
      hiddenInput.value = formatDateTime(selectedDate);
      displayInput.value = dateTimeFormatter.format(selectedDate);
    } else {
      selectedDate.setHours(0, 0, 0, 0);
      hiddenInput.value = formatDateOnly(selectedDate);
      displayInput.value = dateFormatter.format(selectedDate);
    }

    render();
  }

  function updateDisplay() {
    if (!selectedDate) {
      displayInput.value = '';
      return;
    }

    if (showTime) {
      displayInput.value = dateTimeFormatter.format(selectedDate);
      return;
    }

    displayInput.value = dateFormatter.format(selectedDate);
  }

  function parseStoredValue(value, includeTime) {
    if (!value) {
      return null;
    }

    const normalized = value.trim();
    if (!normalized) {
      return null;
    }

    if (!includeTime) {
      const dateParts = normalized.split(/[T\s]/)[0].split('-');
      if (dateParts.length !== 3) {
        return null;
      }

      const year = parseInt(dateParts[0], 10);
      const month = parseInt(dateParts[1], 10) - 1;
      const day = parseInt(dateParts[2], 10);
      return new Date(year, month, day, 0, 0, 0, 0);
    }

    const parsed = new Date(normalized);
    return Number.isNaN(parsed.getTime()) ? null : parsed;
  }

  function formatDateOnly(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  function formatDateTime(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}:00`;
  }

  function isSameDay(left, right) {
    return left.getFullYear() === right.getFullYear()
      && left.getMonth() === right.getMonth()
      && left.getDate() === right.getDate();
  }
}
