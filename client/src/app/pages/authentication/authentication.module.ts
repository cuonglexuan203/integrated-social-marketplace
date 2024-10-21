import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { NbAuthJWTToken, NbAuthModule, NbPasswordAuthStrategy } from "@nebular/auth";
import { environment } from "../../../environments/endpoint";

export const authOptions = {
    name: 'email',
    baseEndpoint: environment.apiAuth,
    login: {
        endpoint: '/login',
        method: 'post',
        requireValidToken: true,
        redirect: {
            success: '/home',
            failure: 'login',
        }
    },

    logout: {
        endpoint: '/logout',
        method: 'post',
    },
    token: {
        class: NbAuthJWTToken,
        key: 'token',
    }
}

@NgModule({
    declarations: [],
    imports: [
        NbAuthModule.forRoot({
            strategies: [
                NbPasswordAuthStrategy.setup(authOptions),
            ],
            forms: {
                login: {
                    strategy: 'email',
                    rememberMe: true,
                }
            },
        }),
        CommonModule,
    ],
})

export class AuthenticationModule { }