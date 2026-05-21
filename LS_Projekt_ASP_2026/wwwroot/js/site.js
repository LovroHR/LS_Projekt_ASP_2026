// Shared UI scripts for the app.

document.addEventListener('DOMContentLoaded', function () {
	initAutocompleteControls();
});

function initAutocompleteControls() {
	document.querySelectorAll('[data-autocomplete-source]').forEach(control => {
		const source = control.dataset.autocompleteSource;
		const mode = control.dataset.autocompleteMode || 'entity';
		const minLength = parseInt(control.dataset.autocompleteMinLength || (mode === 'text' ? '1' : '2'), 10);
		const toggleButton = control.querySelector('.autocomplete-toggle');
			const displayInput = control.querySelector('.autocomplete-input');
		const hiddenInput = control.querySelector('.autocomplete-value');
		const menu = control.querySelector('.autocomplete-menu');
		const searchInput = control.querySelector('.autocomplete-search');
		const resultsWrap = control.querySelector('.autocomplete-results');
		const placeholder = control.dataset.autocompletePlaceholder || 'Pretraži...';
			const validationMessage = control.querySelector(`[data-valmsg-for="${hiddenInput ? hiddenInput.name : ''}"]`);
			const requiredMessage = control.dataset.autocompleteRequiredMessage || (mode === 'text' ? 'Odaberite vrijednost s liste.' : 'Odaberite stavku s liste.');

		if (!menu || !resultsWrap || (!toggleButton && !displayInput)) {
			return;
		}

		let timer = null;
		let activeIndex = -1;
		let currentItems = [];
		let isOpen = false;

		const getDisplayControl = () => displayInput || toggleButton;

			const setValidationState = (isValid) => {
				const visibleControl = getDisplayControl();
				if (visibleControl) {
					visibleControl.classList.toggle('is-invalid', !isValid);
				}

				if (searchInput) {
					searchInput.classList.toggle('is-invalid', !isValid && isOpen);
				}

				if (validationMessage) {
					validationMessage.textContent = isValid ? '' : requiredMessage;
				}
			};

			const hasSelection = () => {
				if (mode === 'entity') {
					return !!(hiddenInput && hiddenInput.value.trim());
				}

				return !!(hiddenInput && hiddenInput.value.trim());
			};

			const validateControl = () => {
				setValidationState(hasSelection());
			};

		const hideMenu = () => {
			menu.classList.remove('show');
			menu.setAttribute('aria-hidden', 'true');
			resultsWrap.innerHTML = '';
			activeIndex = -1;
			currentItems = [];
			isOpen = false;
		};

		const showMenu = () => {
			menu.classList.add('show');
			menu.setAttribute('aria-hidden', 'false');
			isOpen = true;
			if (searchInput) {
				window.setTimeout(() => searchInput.focus(), 0);
			}
		};

		const setDisplayText = (text) => {
			if (toggleButton) {
				toggleButton.textContent = text || placeholder;
				return;
			}

			if (displayInput) {
				displayInput.value = text || '';
			}
		};

		const renderMenu = (items) => {
			currentItems = items;
			activeIndex = -1;
			resultsWrap.innerHTML = '';

			if (!items.length) {
				showMenu();
				const empty = document.createElement('div');
				empty.className = 'autocomplete-empty text-muted small px-2 py-2';
				empty.textContent = 'Nema rezultata';
				resultsWrap.appendChild(empty);
				return;
			}

			items.forEach((item, index) => {
				const option = document.createElement('button');
				option.type = 'button';
				option.className = 'dropdown-item autocomplete-option';
				option.dataset.index = String(index);

				const title = document.createElement('div');
				title.className = 'fw-semibold';
				title.textContent = item.label || item.value || '';
				option.appendChild(title);

				if (item.secondary) {
					const secondary = document.createElement('div');
					secondary.className = 'small text-muted';
					secondary.textContent = item.secondary;
					option.appendChild(secondary);
				}

				option.addEventListener('mousedown', function (event) {
					event.preventDefault();
					applySelection(item);
				});

				resultsWrap.appendChild(option);
			});

			showMenu();
		};

		const applySelection = (item) => {
			const label = item.label || item.value || '';
			setDisplayText(label);

			if (hiddenInput) {
				hiddenInput.value = item.id != null ? String(item.id) : (item.value || '');
			}

				setValidationState(true);
			hideMenu();
		};

		const search = async (query, force = false) => {
			const normalized = query.trim();

			if (!force && normalized.length < minLength) {
				renderMenu([]);
				return;
			}

			const url = new URL(source, window.location.origin);
			url.searchParams.set('q', normalized);

			const response = await fetch(url.toString(), { headers: { Accept: 'application/json' } });
			if (!response.ok) {
				renderMenu([]);
				return;
			}

			const items = await response.json();
			renderMenu(items);
		};

		if (displayInput) {
			displayInput.addEventListener('input', function () {
				if (hiddenInput && mode === 'entity') {
					hiddenInput.value = '';
				}
					setValidationState(true);

            clearTimeout(timer);
				timer = setTimeout(() => search(displayInput.value), 250);
			});
		}

		if (displayInput) {
			displayInput.addEventListener('focus', function () {
				if (displayInput.value.trim().length >= minLength) {
					clearTimeout(timer);
					timer = setTimeout(() => search(displayInput.value), 0);
				}
			});
		}

		if (toggleButton) {
			toggleButton.addEventListener('click', function (event) {
				event.preventDefault();
				if (isOpen) {
					hideMenu();
					return;
				}

				showMenu();
				if (searchInput) {
					searchInput.value = displayInput ? displayInput.value : '';
					search(searchInput.value, true);
				} else if (displayInput) {
					search(displayInput.value, true);
				} else {
					search('', true);
				}
			});
		}

		if (searchInput) {
			searchInput.addEventListener('input', function () {
				if (hiddenInput && mode === 'entity') {
					hiddenInput.value = '';
				}
					setValidationState(true);

				clearTimeout(timer);
				timer = setTimeout(() => search(searchInput.value), 250);
			});

			searchInput.addEventListener('keydown', function (event) {
				if (!menu.classList.contains('show')) {
					return;
				}

				if (event.key === 'ArrowDown') {
					event.preventDefault();
					activeIndex = Math.min(activeIndex + 1, currentItems.length - 1);
					updateActiveOption();
				} else if (event.key === 'ArrowUp') {
					event.preventDefault();
					activeIndex = Math.max(activeIndex - 1, 0);
					updateActiveOption();
				} else if (event.key === 'Enter') {
					if (activeIndex >= 0 && currentItems[activeIndex]) {
						event.preventDefault();
						applySelection(currentItems[activeIndex]);
					}
				} else if (event.key === 'Escape') {
					hideMenu();
				}
			});
		}

		if (displayInput) {
			displayInput.addEventListener('keydown', function (event) {
			if (!menu.classList.contains('show')) {
				return;
			}

			if (event.key === 'ArrowDown') {
				event.preventDefault();
				activeIndex = Math.min(activeIndex + 1, currentItems.length - 1);
				updateActiveOption();
			} else if (event.key === 'ArrowUp') {
				event.preventDefault();
				activeIndex = Math.max(activeIndex - 1, 0);
				updateActiveOption();
			} else if (event.key === 'Enter') {
				if (activeIndex >= 0 && currentItems[activeIndex]) {
					event.preventDefault();
					applySelection(currentItems[activeIndex]);
				}
			} else if (event.key === 'Escape') {
				hideMenu();
			}
			});
		}

			const blurTarget = displayInput || searchInput || toggleButton;
			if (blurTarget) {
				blurTarget.addEventListener('blur', function () {
					window.setTimeout(() => {
						if (!control.contains(document.activeElement)) {
							validateControl();
							hideMenu();
						}
					}, 150);
				});
			}

		menu.addEventListener('mousemove', function (event) {
			const option = event.target.closest('.autocomplete-option');
			if (!option) {
				return;
			}

			activeIndex = parseInt(option.dataset.index || '-1', 10);
			updateActiveOption();
		});

		const updateActiveOption = () => {
			resultsWrap.querySelectorAll('.autocomplete-option').forEach(option => option.classList.remove('active'));

			const activeOption = resultsWrap.querySelector(`.autocomplete-option[data-index="${activeIndex}"]`);
			if (activeOption) {
				activeOption.classList.add('active');
			}
		};

		hideMenu();
			validateControl();
	});
}
