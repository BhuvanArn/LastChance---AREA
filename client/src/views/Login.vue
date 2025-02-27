<template>
    <body>
        <div class="main-container" :class="{ 'mobile-container': mobile }">
            <div class="main-container-top">
                <h2 class="login-title-txt">Login</h2>
                <div class="filler01"></div>
                <img src="@/assets/logo.png" class="logo" alt="logo">
            </div>
            <div class="main-container-bottom">
                <div class="email-container">
                    <img src="@/assets/user.png" class="img-style1" alt="user">
                    <input type="text" class="input-style1" placeholder="Email" v-model="email">
                </div>
                <div class="pwd-container">
                    <img src="@/assets/key.png" class="img-style1" alt="password">
                    <input type="password" class="input-style1" placeholder="Password" v-model="password">
                </div>
                <button @click="loginClient" class="login-btn">LOGIN</button>
                <div v-if="errorMessage" class="error-message">
                    <span>{{ errorMessage }}</span>
                </div>
                <div class="or-filler">
                    <hr class="or-hr">
                    <p class="or-txt">or</p>
                    <hr class="or-hr">
                </div>
                <div class="oauth-container">
                    <div class="oauth-ind-container-fk">
                        <img src="@/assets/oauth/facebook_logo.png" class="img-style2" alt="facebook connection">
                    </div>
                    <div class="oauth-ind-container-gg" @click="signInWithGoogle">
                        <img src="@/assets/oauth/google_logo.png" class="img-style2" alt="google connection">
                    </div>
                    <div class="oauth-ind-container-mc">
                        <img src="@/assets/oauth/microsoft_logo.png" class="img-style2" alt="microsoft connection">
                    </div>
                    <div class="oauth-ind-container-dc">
                        <img src="@/assets/oauth/discord_logo.png" class="img-style2" alt="discord connection">
                    </div>
                </div>
                <div class="link"><p class="txt-link" @click="navigateToRegister">Register</p></div>
                <div class="link"><p class="txt-link" @click="navigateToForgotPwd">Forgot password ?</p></div>
            </div>
        </div>
    </body>
</template>

<script>
import { getCookie, removeCookie, setCookie } from '@/utils/cookies';

export default {
    name: 'Login',
    data() {
        return {
            windowWidth: false,
            mobile: false,
            email: '',
            password: '',
            errorMessage: '',
            client_id: import.meta.env.VITE_APP_GOOGLE_CLIENT_ID,
        }
    },
    mounted() {
        window.addEventListener('resize', this.checkScreen);
        this.checkScreen();
    },
    methods: {
        navigateToRegister(event) {
            event.preventDefault()
            window.location.href = this.$router.resolve({ name: 'register' }).href;
        },
        navigateToHome(event) {
            event.preventDefault()
            window.location.href = this.$router.resolve({ name: 'home' }).href;
        },
        navigateToForgotPwd(event) {
            event.preventDefault()
            window.location.href = this.$router.resolve({ name: 'forgot-password' }).href;
        },
        checkScreen() {
            this.windowWidth = window.innerWidth;
            if (this.windowWidth <= 960) {
                this.mobile = true;
            } else {
                this.mobile = false;
            }
        },

		async signInWithGoogle() {
			if (!window.google || !google.accounts) {
				console.error('Google Identity Services not initialized');
			    return;
			}
			google.accounts.id.initialize({
				client_id: this.client_id,
				callback: this.handleCredentialResponse,
			});
			google.accounts.id.prompt()
		},

        async handleCredentialResponse(response) {
            if (response.error) {
                console.error(response.error);
                return;
            }
            const credential = response.credential;
            const decodedToken = JSON.parse(atob(credential.split('.')[1]));
			const email = decodedToken.email;
			const name = decodedToken.name;
            try {
                const res = await this.$axios.post('/auth/google-login', {
                    Token: credential,
                });
                setCookie('GoogleUsername', name, 7);
                setCookie('GoogleEmail', email, 7);
                setCookie('token', res.data, 7);
                setCookie('Status', 'Logged In', 7);
                setCookie('AccountType', 'Google', 7);

                const expirationTime = new Date().getTime() + (7 * 24 * 60 * 60 * 1000); // 7d

                setCookie('expirationTime', expirationTime, 7);
                window.location.href = this.$router.resolve({ name: 'panel' }).href;
            } catch (error) {
                console.error(error);
            }
        },

        async loginClient(event) {
            if (!this.email || !this.password) {
                this.errorMessage = 'Please fill all the fields';
                return;
            }
            if (!this.email.match(/^\S+@\S+\.\S+$/)) {
                this.errorMessage = 'Please enter a valid email';
                return;
            }
            this.errorMessage = '';
            try {
                const response = await this.$axios.post('/auth/login', {
                    Email: this.email,
                    Password: this.password
                });
                if (response.status != 200 || !response.data) {
                    this.errorMessage = 'Invalid email or password';
                    return;
                }
                const token = response.data;
                removeCookie('token');
                setCookie('token', token, 7);
                setCookie('Status', 'Logged In', 7);
                setCookie('AccountType', 'Area', 7);
                const expirationTime = new Date().getTime() + (7 * 24 * 60 * 60 * 1000); // 7d
                setCookie('expirationTime', expirationTime, 7);
                window.location.href = this.$router.resolve({ name: 'panel' }).href;
            } catch (error) {
                console.error(error);
                this.errorMessage = 'An error occurred';
            }
        }
    }
}
</script>

<style>
body {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100vh;
    margin: 0;
    padding: 0;
    background-color: #efefef;
}

.main-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    width: 33rem;
    height: 35rem;
    background-color: transparent;
}

.mobile-container {
    width: 25rem;
    height: 35rem;
}

.main-container-bottom {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 80%;
    background-color: #efefef;
    border-bottom-left-radius: 10px;
    border-bottom-right-radius: 10px;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.1);
    gap: 0.5rem;
}

.main-container-top {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 20%;
    background-color: #bcc1ba;
    border-top-left-radius: 10px;
    border-top-right-radius: 10px;
    border-bottom-color: black;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.1);
    border: 1px solid transparent;
    border-bottom-color: #000000;
}

.login-title-txt {
    font-size: 3rem;
    color: #313030;
    margin: 0;
    padding: 0;
    margin-left: 1.5rem;
    margin-right: 1rem;
    font-family: 'inter', sans-serif;
    font-weight: 400;
}

.logo {
    width: 5rem;
    height: 5rem;
    margin-left: 1rem;
    margin-right: 1.5rem;
}

.filler01 {
    width: 100%;
    height: 100%;
    background-color: transparent;
}

.email-container {
    display: flex;
    flex-direction: row;
    align-items: center;
    background-color: transparent;
    height: 3rem;
    width: 23rem;
}

.pwd-container {
    display: flex;
    flex-direction: row;
    align-items: center;
    background-color: transparent;
    height: 3rem;
    width: 23rem;
    margin-bottom: 1rem;
}

.img-style1 {
    width: 1.5rem;
    height: 1.5rem;
    margin-left: 1rem;
    margin-right: 1rem;
}

.img-style2 {
    width: 2.1rem;
    height: 2.1rem;
    margin-left: 1rem;
    margin-right: 1rem;
}

.input-style1 {
    width: 17rem;
    height: 2rem;
    background-color: transparent;
    border: none;
    border-bottom: 2px solid #bcc1ba;
    font-family: 'inter', sans-serif;
    font-size: medium;
    color: #313030;
    margin-right: 1rem;
    outline: none;
    transition: border-bottom 0.35s ease;
}

.link {
    font-family: 'inter', sans-serif;
    font-size: medium;
    color: #28728B;
    cursor: pointer;
}

.login-btn {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #28728B;
    border-radius: 5px;
    border: none;
    width: 8rem;
    height: 2rem;
    color: #efefef;
    font-family: 'inter', sans-serif;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 0.5rem;
    margin-bottom: 1rem;
}

.login-btn:hover {
    background-color: #3a9cb1;
}

.login-btn:active {
    background-color: #2e7f8f;
}

.or-filler {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    width: 23rem;
    height: 2rem;
    background-color: transparent;
}

.or-hr {
    width: 10rem;
    background-color: #bcc1ba;
}

.or-txt {
    font-family: 'inter', sans-serif;
    font-size: medium;
    color: #313030;
    margin-left: 0.5rem;
    margin-right: 0.5rem;
}

.oauth-container {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    width: 20rem;
    height: 4rem;
    background-color: transparent;
    gap: 1rem;
    margin-bottom: 1rem;
}

.oauth-ind-container-fk {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 4rem;
    height: 3rem;
    border-radius:5px;
    background-color: rgb(65, 153, 230);
    cursor: pointer;
}

.oauth-ind-container-fk:hover {
    background-color: rgb(50, 122, 184);
}

.oauth-ind-container-fk:active {
    background-color: rgb(35, 91, 138);
}

.oauth-ind-container-gg {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 4rem;
    height: 3rem;
    border-radius:5px;
    background-color: rgb(255, 255, 255);
    cursor: pointer;
}

.oauth-ind-container-mc {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 4rem;
    height: 3rem;
    border-radius:5px;
    background-color: rgb(255, 255, 255);
    cursor: pointer;
}

.oauth-ind-container-gg:hover, .oauth-ind-container-mc:hover {
    background-color: rgb(240, 240, 240);
}

.oauth-ind-container-gg:active, .oauth-ind-container-mc:active {
    background-color: rgb(225, 225, 225);
}

.oauth-ind-container-dc {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 4rem;
    height: 3rem;
    border-radius:5px;
    background-color: rgb(55, 133, 222);
    cursor: pointer;
}

.oauth-ind-container-dc:hover {
    background-color: rgb(40, 102, 170);
}

.oauth-ind-container-dc:active {
    background-color: rgb(25, 65, 107);
}

.txt-link {
    font-family: 'inter', sans-serif;
    font-weight: 300;
    cursor: pointer;
    margin: 0;
    padding: 0;
    color: #28728B;
    border-bottom: 1px solid #28728B;
}

.error-message {
    color: red;
    font-family: 'inter', sans-serif;
    font-size: medium;
    margin-bottom: -1rem;
}

</style>