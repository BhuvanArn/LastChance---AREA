<template>
    <body>
        <div class="main-container" :class="{ 'mobile-container': mobile }">
            <div class="main-container-top">
                <h2 class="login-title-txt">Register</h2>
                <div class="filler01"></div>
                <img src="@/assets/logo.png" class="logo">
            </div>
            <div class="main-container-bottom">
                <div class="username-container">
                    <img src="@/assets/user.png" class="img-style1">
                    <input type="text" class="input-style1" placeholder="Name" v-model="name">
                </div>
                <div class="email-container">
                    <img src="@/assets/user.png" class="img-style1">
                    <input type="text" class="input-style1" placeholder="Your Email" v-model="email">
                </div>
                <div class="pwd-container">
                    <img src="@/assets/key.png" class="img-style1">
                    <input type="password" class="input-style1" placeholder="Your Password" v-model="password">
                </div>
                <button @click="registerClient" class="login-btn">REGISTER</button>
                <div v-if="errorMessage" class="error-message">
                    <span>{{ errorMessage }}</span>
                </div>
                <div class="filler02"></div>
                <p class="txt-link" @click="navigateToLogin">Already have an account ?</p>
            </div>
        </div>
    </body>
</template>

<script>

export default {
    name: 'Register',
    data() {
        return {
            windowWidth: false,
            mobile: false,
            email: '',
            password: '',
            name: '',
            errorMessage: ''
        }
    },
    mounted() {
        window.addEventListener('resize', this.checkScreen);
        this.checkScreen();
    },
    methods: {
        navigateToLogin(event) {
            event.preventDefault()
            window.location.href = this.$router.resolve({ name: 'login' }).href;
        },
        checkScreen() {
            this.windowWidth = window.innerWidth;
            if (this.windowWidth <= 960) {
                this.mobile = true;
            } else {
                this.mobile = false;
            }
        },
        async registerClient() {
            if (!this.email || !this.password) {
                this.errorMessage = 'Please fill in all fields';
                return;
            }
            // check if email is valid
            if (!this.email.match(/^\S+@\S+\.\S+$/)) {
                this.errorMessage = 'Please enter a valid email address';
                return;
            }

            this.errorMessage = '';

            try {
                console.log('Registering client...');
                const response = await this.$axios.post('/auth/register', {
                    Email: this.email,
                    Password: this.password,
                    ConfirmedPassword: this.password,
                    Username: this.name
                });
                console.log(response);

                // Redirect to login page
                this.$router.push({ name: 'login' });
            } catch (error) {
                console.error(error);
                this.errorMessage = 'An error occurred. Please try again later.';
            };
        },
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

.filler02 {
    width: 100%;
    height: 3rem;
    background-color: transparent;
}

.username-container {
    display: flex;
    flex-direction: row;
    align-items: center;
    background-color: transparent;
    height: 3rem;
    width: 23rem;
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
    margin-bottom: 5px;
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
    margin-bottom: -2rem;
}

</style>
