
document.addEventListener('DOMContentLoaded', () => {

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
            }
           
        })
    })
})