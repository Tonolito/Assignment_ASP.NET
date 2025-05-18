
    document.addEventListener('DOMContentLoaded', () => {
        const previewSize = 150

        // ÄNDRING.
        // DROPDOWN
        initializeDropdowns();
        updateRelativeTimes();
        setInterval(updateRelativeTimes, 60000);

        const form = document.querySelector("form");

        if (!form) return;

        const fields = form.querySelectorAll("input[data-val='true']");
        console.log("Found fields:", fields.length);

        // Lägg till event listener för varje fält
        fields.forEach(field => {
            console.log("Adding event listener to:", field.name);

            field.addEventListener("input", function () {
                validateField(field); // Validera varje fält när användaren skriver
            });
        });

        form.addEventListener("submit", function (event) {
            let isValid = true;

            fields.forEach(field => {
                validateField(field); // Kör validering på varje fält

                // Kontrollera om fältet har felklass och stoppa submit om det finns fel
                if (field.classList.contains("input-validation-error")) {
                    isValid = false; // Om någon fält har felklass, stoppa submit
                }
            });

            // Om inte alla fält är giltiga, stoppa submit
            if (!isValid) {
                event.preventDefault(); // Stoppa formuläret från att skickas
                console.log("Form submission blocked due to validation errors.");
            }
        });

    

        // Open modals
        const modalButtons = document.querySelectorAll('[data-modal="true"]');

        modalButtons.forEach(button => {
            button.addEventListener('click', async () => {
                const modalTarget = button.getAttribute('data-target');
                const modal = document.querySelector(modalTarget);

                if (modal) {
                    modal.style.display = 'flex';
                    console.log('Open');
                    if (modalTarget === '#editProjectModal' && typeof initEditModalTags === 'function') {
                        initEditModalTags();
                    }
                    if (modalTarget === '#addProjectModal' && typeof initAddModalTags === 'function') {
                        initAddModalTags();
                    }
                }
            });
        });

        const editMemberButtons = document.querySelectorAll('[data-modal="true"][data-target="#editMemberModal"]');

        editMemberButtons.forEach(button => {
            button.addEventListener('click', async () => {
                const memberId = button.getAttribute('data-id');

                try {
                    const response = await fetch(`/members/edit/${memberId}`);
                    const data = await response.json();

                    if (data) {
                        const form = document.querySelector('#editMemberForm');

                        form.querySelector('input[name="Id"]').value = data.id;
                        form.querySelector('input[name="FirstName"]').value = data.firstName;
                        form.querySelector('input[name="LastName"]').value = data.lastName;
                        form.querySelector('input[name="Email"]').value = data.email;
                        form.querySelector('input[name="PhoneNumber"]').value = data.phoneNumber;
                        form.querySelector('input[name="JobTitle"]').value = data.jobTitle;
                        form.querySelector('input[name="Address"]').value = data.address;

                        const imagePreview = form.querySelector('.image-preview');
                        imagePreview.src = data.image || '';

                        const modal = document.querySelector('#editMemberModal');
                        modal.style.display = 'flex';

                        if (!form.hasAttribute('data-submit-bound')) {
                            form.addEventListener('submit', async (e) => {
                                e.preventDefault();
                                const formData = new FormData(form);

                                try {
                                    const updateResponse = await fetch(`/members/edit`, {
                                        method: 'PUT',
                                        body: formData
                                    });

                                    if (updateResponse.ok) {
                                        modal.style.display = 'none';
                                        window.location.reload();
                                    } else {
                                        const errorData = await updateResponse.json();
                                        alert(`Error updating member: ${errorData.error || "An unexpected error occurred"}`);
                                    }
                                } catch (submitError) {
                                    console.log('Error submitting the form', submitError);
                                }
                            });
                            form.setAttribute('data-submit-bound', 'true');
                        }
                    }

                } catch (fetchError) {
                    console.log('Error loading member data', fetchError);
                }
            });
        });

        // ====== CLIENT ======
        const editClientButtons = document.querySelectorAll('[data-modal="true"][data-target="#editClientModal"]');

        editClientButtons.forEach(button => {
            button.addEventListener('click', async () => {
                const clientId = button.getAttribute('data-id');

                try {
                    const response = await fetch(`/clients/edit/${clientId}`);
                    const data = await response.json();

                    if (data) {
                        const form = document.querySelector('#editClientForm');

                        form.querySelector('input[name="Id"]').value = data.id;
                        form.querySelector('input[name="ClientName"]').value = data.clientName;
                        form.querySelector('input[name="Email"]').value = data.email;
                        form.querySelector('input[name="Location"]').value = data.location;
                        form.querySelector('input[name="Phone"]').value = data.phone;

                        const imagePreview = form.querySelector('.image-preview');
                        imagePreview.src = data.image || '';

                        const modal = document.querySelector('#editClientModal');
                        modal.style.display = 'flex';

                        if (!form.hasAttribute('data-submit-bound')) {
                            form.addEventListener('submit', async (e) => {
                                e.preventDefault();
                                const formData = new FormData(form);

                                try {
                                    const response = await fetch(`/clients/edit`, {
                                        method: 'PUT',
                                        body: formData
                                    });

                                    if (response.ok) {
                                        modal.style.display = 'none';
                                        window.location.reload();
                                    } else {
                                        const data = await response.json();
                                        alert(`Error updating client: ${data.error || "An unexpected error occurred"}`);
                                    }
                                } catch (error) {
                                    console.log('Error submitting the form', error);
                                }
                            });
                            form.setAttribute('data-submit-bound', 'true');
                        }
                    }
                } catch (error) {
                    console.log('Error loading client data', error);
                }
            });
        });


        const editProjectButtons = document.querySelectorAll('[data-modal="true"][data-target="#editProjectModal"]');

        editProjectButtons.forEach(button => {
            button.addEventListener('click', async () => {
                const projectId = button.getAttribute('data-id');

                try {
                    const response = await fetch(`/projects/edit/${projectId}`);
                    const data = await response.json();

                    if (data) {
                        const form = document.querySelector('#editProjectForm');
                        console.log(data)

                        form.querySelector('input[name="Id"]').value = data.id;
                        form.querySelector('input[name="ProjectName"]').value = data.projectName;
                        form.querySelector('input[name="Budget"]').value = data.budget;

                        form.querySelector('input[name="StartDate"]').value = formatDateForInput(data.startDate);
                        form.querySelector('input[name="EndDate"]').value = formatDateForInput(data.endDate);

                        const preselectedClient = data.client ? [{
                            id: data.client.id,
                            clientName: data.client.clientName,
                            imageUrl: data.client.image
                        }] : [];

                        initTagSelector({
                            containerId: 'tagged-clients-edit',
                            inputId: 'client-search-edit',
                            resultId: 'client-search-results-edit',
                            searchUrl: (query) => '/clients/search-clients?term=' + encodeURIComponent(query),
                            displayProperty: 'clientName',
                            imageProperty: 'imageUrl',
                            avatarFolder: '',
                            tagClass: 'client-tag',
                            emptyMessage: 'No clients found.',
                            preselected: preselectedClient,
                            selectedInputIds: 'SelectedClientId'
                        });

                        const preselectedMembers = (data.members || []).map(member => ({
                            id: member.id,
                            fullName: `${member.firstName} ${member.lastName}`,
                            imageUrl: member.image
                        }));


                        initTagSelector({
                            containerId: 'tagged-users-edit',
                            inputId: 'user-search-edit',
                            resultId: 'user-search-results-edit',
                            searchUrl: (query) => '/members/search-members?term=' + encodeURIComponent(query),
                            displayProperty: 'fullName',
                            imageProperty: 'imageUrl',
                            avatarFolder: '',
                            tagClass: 'user-tag',
                            emptyMessage: 'No users found.',
                            preselected: preselectedMembers,
                            selectedInputIds: 'SelectedMemberIds'
                        });


                        
                        if (quillEditorEdit) {
                            console.log(data.description)
                            quillEditorEdit.root.innerHTML = data.description || '';
                            form.querySelector('#Description-edit').value = data.description || '';
                        }
                        //form.querySelector('input[name="ClientName"]').value = data.clientName;
                        //form.querySelector('input[name="MemberIds"]').value = data.memberIds;


                        const imagePreview = form.querySelector('.image-preview');
                        imagePreview.src = data.image || '';

                        const modal = document.querySelector('#editProjectModal');
                        modal.style.display = 'flex';

                        if (!form.hasAttribute('data-submit-bound')) {
                            form.addEventListener('submit', async (e) => {
                                e.preventDefault();
                                const formData = new FormData(form);

                                try {
                                    const response = await fetch(`/projects/edit`, {
                                        method: 'PUT',
                                        body: formData
                                    });

                                    if (response.ok) {
                                        modal.style.display = 'none';
                                        window.location.reload();
                                    } else {
                                        const data = await response.json();
                                        alert(`Error updating project: ${data.error || "An unexpected error occurred"}`);
                                    }
                                } catch (error) {
                                    console.log('Error submitting the form', error);
                                }
                            });
                            form.setAttribute('data-submit-bound', 'true');
                        }
                    }
                } catch (error) {
                    console.log('Error loading project data', error);
                }
            });
        });




        // Close Modals
        const closeButtons = document.querySelectorAll('[data-close="true"]')
        closeButtons.forEach(button => {
            button.addEventListener('click', () => {
                const modal = button.closest('.modal-container')
                if (modal) {
                    modal.style.display = 'none'
                    console.log('Close')

                    modal.querySelectorAll('form').forEach(form => {
                        form.reset()

                        const imagePreview = form.querySelector('.image-preview')
                        if (imagePreview) {
                            imagePreview.src = ''
                        }

                        const imagePreviewer = form.querySelector('.image-previewer')
                        if (imagePreviewer) {
                            imagePreviewer.classList.remove('selected')
                        }
                    })
                }
           
            })
        })

    


        // Handel image previewer

        document.querySelectorAll('.image-previewer').forEach(previewer => {
            const fileInput = previewer.querySelector('input[type="file"]')
            const imagePreview = previewer.querySelector('.image-preview')

            previewer.addEventListener('click', () => fileInput.click())

            fileInput.addEventListener('change', ({ target: { files } }) => {
                const file = files[0]
                if (file) {
                    processImage(file, imagePreview, previewer, previewSize)
                }
            })
        })



        // Handle submit forms

        const forms = document.querySelectorAll('form')
        forms.forEach(form => {
            form.addEventListener('submit', async (e) => {
                e.preventDefault()

        
                clearErrorMessages(form)
            

                const formData = new FormData(form)
            

                try {
                    const res = await fetch(form.action, {
                        method: 'post',
                        body: formData
                    })
                    if (res.ok) {
                        const modal = form.closest('.modal')
                        if (modal)
                            modal.style.display = 'none';

                            window.location.reload()
                    }
                    else if (!res.status === 400) {
                        const data = await res.json()

                        console.log(data)

                        if (data.errors) {
                            Object.keys(data.errors).forEach(key => {
                                const input = form.querySelector(`[name=${key}]`)
                                if (input) {
                                    input.classList.add('input-validation-error')
                                }

                                const span = form.querySelector(`[data-valmsg-for="${key}"]`)
                                if (span) {
                                    span.innerText = data.errors[key].join('\n')
                                    span.classList.add('field-validation-error')
                                }                        })
                        }
                    }
                }
                catch {
                    console.log('error submitting the form')
                }
            })
        })

        // Chatgpt för att delete knappen ska fungera
        const deleteButtons = document.querySelectorAll('.dropdown-option-delete');
        deleteButtons.forEach(button => {
            button.addEventListener('click', async (event) => {
                const parent = button.closest('.dropdown-client') || button.closest('.dropdown-member') || button.closest('.dropdown-project');
                if (!parent) return;

                const parentId = parent.id;
                const entityId = parentId.split('-').slice(2).join('-'); // Fångar ID:t efter "client-dropdown-" osv

                // Avgör typ
                let entityType;
                if (parentId.startsWith('client')) {
                    entityType = 'client';
                } else if (parentId.startsWith('member')) {
                    entityType = 'member';
                } else if (parentId.startsWith('project')) {
                    entityType = 'project';
                } else {
                    alert('Unknown entity type.');
                    return;
                }

                const isConfirmed = confirm(`Are you sure you want to delete this ${entityType}?`);
                if (!isConfirmed) return;

                try {
                    const response = await fetch(`https://localhost:7023/${entityType}s/delete/${entityId}`, {
                        method: 'DELETE',
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    });

                    if (response.ok) {
                        parent.remove();
                        alert(`${entityType.charAt(0).toUpperCase() + entityType.slice(1)} deleted successfully.`);
                    } else {
                        const text = await response.text();
                        try {
                            const data = JSON.parse(text);
                            alert(`Error deleting ${entityType}: ${data.error || "An unexpected error occurred"}`);
                        } catch (e) {
                            alert(`Error deleting ${entityType}: No valid response data.`);
                        }
                    }
                } catch (error) {
                    console.error(`Error deleting ${entityType}:`, error);
                    alert(`Error deleting ${entityType}.`);
                }
            });
        });






    })

function formatDateForInput(dateString) {
    const date = new Date(dateString);
    const yyyy = date.getFullYear();
    const mm = String(date.getMonth() + 1).padStart(2, '0');
    const dd = String(date.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
}



    function initializeDarkmode() {
        const darkmodeSwitch = document.querySelector("#darkmode-switch");
        if (!darkmodeSwitch) return;

        const hasDarkmode = localStorage.getItem("darkmode");

        if (hasDarkmode == null) {
            if (window.matchMedia("(prefers-color-scheme: dark)").matches) {
                enableDarkmode();
            } else {
                disableDarkmode();
            }
        } else if (hasDarkmode === "on") {
            enableDarkmode();
        } else {
            disableDarkmode();
        }

        darkmodeSwitch.addEventListener("change", () => {
            if (darkmodeSwitch.checked) {
                enableDarkmode();
                localStorage.setItem("darkmode", "on");
            } else {
                disableDarkmode();
                localStorage.setItem("darkmode", "off");
            }
        });

        function enableDarkmode() {
            darkmodeSwitch.checked = true;
            document.documentElement.classList.add("dark");
        }

        function disableDarkmode() {
            darkmodeSwitch.checked = false;
            document.documentElement.classList.remove("dark");
        }
    }
    function clearErrorMessages(form) {
        form.querySelectorAll('[data-val="true"]').forEach(input => {
            input.classList.remove('input-validation-error')
        })
        form.querySelectorAll('[data-valmsg-for]').forEach(span => {
            span.innerText = ''
            span.classList.remove('field-validation-error')
        })
    }

    function addErrorMessage(key, errorMessage) {
        const input = form.querySelector(`[name=${key}]`)
        if (input) {
            input.classList.add('input-validation-error')
        }

        const span = form.querySelector(`[data-valmsg-for="${key}"]`)
        if (span) {
            span.innerText = errorMessage
            span.classList.add('field-validation-error')
        }
    }

    async function loadImage(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader()

            reader.onerror = () => reject(new Error("Failed to load file."))
            reader.onload = (e) => {
                const img = new Image()
                img.onerror = () => reject(new Error("Faild to load gmage"))
                img.onload = () => resolve(img)
                img.src = e.target.result
            }

            reader.readAsDataURL(file)
        })
    }

    async function processImage(file, imagePreview, previewer, previewSize = 150) {
        try {
            const img = await loadImage(file)
            const canvas = document.createElement('canvas')
            canvas.width = previewSize
            canvas.height = previewSize

            const ctx = canvas.getContext('2d')
            ctx.drawImage(img, 0, 0, previewSize, previewSize)
            imagePreview.src = canvas.toDataURL('image/jpeg')
            previewer.classList.add('selected')
        }
        catch(error) {
            console.log('Failed on image processing', error)
        }
    }
    function validateField(field) {
        console.log("Validating field:", field.name);

        let errorSpan = document.querySelector(`span[data-valmsg-for='${field.name}']`);
        if (!errorSpan) {
            console.log("No error span found for", field.name);
            return;
        }

        let errorMessage = "";
        let value = field.value.trim();

        if (field.hasAttribute("data-val-required") && value === "") {
            errorMessage = field.getAttribute("data-val-required");
            console.log("Required field validation failed:", errorMessage);
        }

        if (field.hasAttribute("data-val-regex") && value !== "") {
            let pattern = new RegExp(field.getAttribute("data-val-regex-pattern"));
            if (!pattern.test(value)) {
                errorMessage = field.getAttribute("data-val-regex");
                console.log("Regex validation failed:", errorMessage);
            }
        }

        if (errorMessage) {
            field.classList.add("input-validation-error");
            errorSpan.classList.remove("field-validation-valid");
            errorSpan.classList.add("field-validation-error");
            errorSpan.textContent = errorMessage;
        } else {
            field.classList.remove("input-validation-error");
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
            errorSpan.textContent = "";
        }

    }

    //DarkMode
    function enableDarkmode() {
        darkmodeSwitch.checked = true;
        document.documentElement.classList.add("dark");
    }

    function disableDarkmode() {
        darkmodeSwitch.checked = false;
        document.documentElement.classList.remove("dark");
    }




    // HANS VIDEO

    function closeALlDropdowns(exceptDorpdown, dropdownElements) {
        dropdownElements.forEach(dropdown => {
            if (dropdown != exceptDorpdown) {
                dropdown.classList.remove('show')
            }
        })
    }
    function initializeDropdowns() {
        const dropdownTriggers = document.querySelectorAll('[data-type="dropdown"]');

        const dropdownElements = new Set();

        dropdownTriggers.forEach(trigger => {
            const targetSelector = trigger.getAttribute('data-target');
            if (targetSelector) {
                const dropdown = document.querySelector(targetSelector);
                if (dropdown) {
                    dropdownElements.add(dropdown);
                }
            }

            trigger.addEventListener('click', (e) => {
                e.stopPropagation();
                const targetSelector = trigger.getAttribute('data-target');
                if (!targetSelector) return;

                const dropdown = document.querySelector(targetSelector);
                if (!dropdown) return;

                closeAllDropdowns(dropdown, dropdownElements);
                dropdown.classList.toggle('show');
            });
        });

        dropdownElements.forEach(dropdown => {
            dropdown.addEventListener('click', (e) => {
                e.stopPropagation();
            });
        });

        document.addEventListener('click', () => {
            closeAllDropdowns(null, dropdownElements);
        });

        function closeAllDropdowns(excludeDropdown, allDropdowns) {
            allDropdowns.forEach(dropdown => {
                if (dropdown !== excludeDropdown) {
                    dropdown.classList.remove('show');
                }
            });
        }

   


    }

    function updateRelativeTimes() {
        const elements = document.querySelectorAll('.notification-item .time');
        const now = new Date();

        elements.forEach(el => {
            const created = new Date(el.getAttribute('data-created'));
            const diff = now - created;
            const diffSeconds = Math.floor(diff / 1000);
            const diffMinutes = Math.floor(diffSeconds / 60);
            const diffHours = Math.floor(diffMinutes / 60);
            const diffDays = Math.floor(diffHours / 24);
            const diffWeeks = Math.floor(diffDays / 7);

            let relativeTime = '';

            if (diffMinutes < 1) {
                relativeTime = '0 min ago';
            } else if (diffMinutes < 60) {
                relativeTime = diffMinutes + ' min ago';
            } else if (diffHours < 2) {
                relativeTime = diffHours + ' hour ago';
            } else if (diffHours < 24) {
                relativeTime = diffHours + ' hours ago';
            } else if (diffDays < 2) {
                relativeTime = diffDays + ' day ago';
            } else if (diffDays < 7) {
                relativeTime = diffDays + ' days ago';
            } else {
                relativeTime = diffWeeks + ' weeks ago';
            }

            el.textContent = relativeTime;
        });
    }
