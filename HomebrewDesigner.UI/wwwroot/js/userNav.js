export default class UserInfoManager {
    constructor() {
        this.navBarNav = document.getElementById('navbarNav');
        this.userInfoDiv = document.getElementById('userInfo');
        this.userDiv = document.getElementById('userDiv');
        this.hrElementId = 'hrUserInfo';
        window.addEventListener('resize', this.checkAndInsertHr.bind(this));
        this.checkAndInsertHr();
    }

    checkAndInsertHr() {
        let hrElement = document.getElementById(this.hrElementId);
        if (window.innerWidth < 991) {
            this.navBarNav.classList.add('mt-4')
            this.userInfoDiv.classList.remove('mx-5');
            this.userDiv.classList.add('text-center');
            if (!hrElement) {

                hrElement = document.createElement('hr');
                hrElement.id = this.hrElementId;

                this.userInfoDiv.insertAdjacentElement('afterbegin', hrElement);
            }
        } else {
            this.navBarNav.classList.remove('mt-4')
            this.userDiv.classList.remove('text-center');

            if (hrElement) {
                hrElement.remove();
            }
        }
    }
}
