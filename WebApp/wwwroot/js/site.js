﻿
document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150


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

    // Lägg till event listener för form submit
    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Validera alla fält vid submit
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
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)


            if (modal) {
                modal.style.display = 'flex'
                console.log('Open')
            }
                
        })
    })

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

    document.querySelectorAll(".icon-trigger").forEach(trigger => {
        trigger.addEventListener("click", (e) => {
            e.stopPropagation(); // Förhindrar att klick utanför stänger direkt

            let dropdown = null;

            // Kontrollera vilken ikon som klickades
            if (trigger.classList.contains("fa-bell")) {
                dropdown = document.querySelector(".notification");
            } else if (trigger.classList.contains("fa-gear")) {
                dropdown = document.querySelector(".profile-dropdown");
            }

            if (dropdown) {
                // Stäng alla andra dropdowns först
                document.querySelectorAll(".dropdown").forEach(d => {
                    if (d !== dropdown) d.style.display = "none";
                });

                // Växla synlighet på den aktuella dropdownen
                dropdown.style.display = dropdown.style.display === "flex" ? "none" : "flex";
            }
        });
    });

    // Klick utanför → stäng alla dropdowns
    document.addEventListener("click", () => {
        document.querySelectorAll(".dropdown").forEach(dropdown => {
            dropdown.style.display = "none";
        });
    });


    // Dropdown för alla cards
    document.querySelectorAll(".dots-container").forEach(button => {
        button.addEventListener("click", (event) => {
            event.stopPropagation(); // Förhindrar att klick utanför stänger direkt

            let dropdown = null;

            // Kontrollera om knappen finns i .upper-card (för project och members)
            const upperCard = button.closest(".upper-card");
            if (upperCard) {
                dropdown = upperCard.querySelector(".dropdown");
            }

            // Om knappen finns i .client-card istället
            const clientCard = button.closest(".client-card");
            if (clientCard) {
                dropdown = clientCard.querySelector(".dropdown-client");
            }

            if (dropdown) {
                // Stäng alla andra dropdowns först
                document.querySelectorAll(".dropdown, .dropdown-client").forEach(d => {
                    if (d !== dropdown) d.style.display = "none";
                });

                // Växla synlighet på denna dropdown
                dropdown.style.display = dropdown.style.display === "flex" ? "none" : "flex";
            }
        });
    });

    // Klicka utanför dropdown → stäng alla dropdowns
    document.addEventListener("click", () => {
        document.querySelectorAll(".dropdown, .dropdown-client").forEach(dropdown => {
            dropdown.style.display = "none";
        });
    });


    // Klicka utanför dropdown → stäng alla dropdowns
    document.addEventListener("click", () => {
        document.querySelectorAll(".dropdown").forEach(dropdown => {
            dropdown.style.display = "none";
        });
    });


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
})

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

