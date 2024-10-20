import { Routes } from '@angular/router';
import { FullComponent } from './layouts/full/full.component';
import { AuthGuard, PermissionGuard } from './pages/authentication/auth-guard.guard';
import { BlankComponent } from './layouts/blank/blank.component';
import { ErrorComponent } from './pages/authentication/error/error.component';
import { ChatComponent } from './pages/chat/chat.component';

export const routes: Routes = [     
    {
        path: '',
        component: FullComponent,
        // canActivate: [AuthGuard],
        // canActivateChild: [PermissionGuard],
        children: [
            {
                path: '',
                redirectTo: '/home',
                pathMatch: 'full',
            },
            {
                path: 'home',
                loadChildren: () => import('./pages/pages.routes').then(m => m.PagesRoutes),
            },
            {
                path: 'chat',
                component: ChatComponent,
            }
        ]
    },
    {
        path: '',
        component: BlankComponent,
        children: [
            {
                path: '',
                loadChildren: () => import('./pages/authentication/authentication.routes').then(m => m.AuthenticationRoutes),
            }
        ]
    },
    {
        path: "error",
        component: ErrorComponent,
    },
    {
        path: '**', redirectTo: '/home'
    }
];
