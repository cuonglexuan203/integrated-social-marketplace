import { Routes } from '@angular/router';
import { FullComponent } from './layouts/full/full.component';
import { AuthGuard, PermissionGuard } from './pages/authentication/auth-guard.guard';
import { ErrorComponent } from './pages/authentication/error/error.component';

export const routes: Routes = [
    {
        path: '',
        component: FullComponent,
        canActivate: [AuthGuard],
        canActivateChild: [PermissionGuard],
        children: [
            {
                path: '',
                redirectTo: '/home',
                pathMatch: 'full'
            },
            {
                path: 'home',
                loadChildren: () => import('./pages/pages.routes').then((m) => m.PagesRoutes),
            }
        ]
    },
    {
        path: '',

    }
];
