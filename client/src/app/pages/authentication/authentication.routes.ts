import { Routes } from '@angular/router';
import { ErrorComponent } from './error/error.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PhoneVerificationComponent } from './phone-verification/phone-verification.component';

export const AuthenticationRoutes: Routes = [
    {
        path: '',
        children: [
            {
                path: '',
                component: LoginComponent,
            },
            {
                path: 'login',
                component: LoginComponent,
            },
            {
                path:'error',
                component: ErrorComponent
            },
            {
                path:'register',
                component: RegisterComponent
            },
            {
                path: 'verify-phone',
                component: PhoneVerificationComponent
            }
        ]
    }
]