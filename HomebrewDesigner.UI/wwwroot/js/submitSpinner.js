export default class SubmitSpinner {

    constructor() {
        this.form = document.getElementById('form');
        this.loader = document.getElementById('loader');
        this.buttonDiv = document.getElementById('buttonDiv');
        this.init();
    }

    init() {
        this.form.addEventListener('submit', this.handleSubmit.bind(this));
    }

    handleSubmit() {
        this.showLoader();

        setTimeout(() => {
            if (this.hasValidationErrors()) {
                this.hideLoader();
            }
        }, 100);
    }

    hasValidationErrors() {
        const validationErrors = Array.from(document.querySelectorAll('.text-validation'));
        return validationErrors.some(element => element.innerHTML.trim() !== '');

    }

    showLoader() {
        this.loader.classList.remove('visually-hidden');
        this.buttonDiv.classList.add('visually-hidden');
    }

    hideLoader() {
        this.loader.classList.add('visually-hidden');
        this.buttonDiv.classList.remove('visually-hidden');
    }
}