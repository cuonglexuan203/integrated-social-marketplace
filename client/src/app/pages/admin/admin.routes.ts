import { Routes } from '@angular/router';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { ReportManagementComponent } from './report-management/report-management.component';
import { UserRoleType } from '../../shared/roles/role.type';
export const AdminRoutes: Routes = [
    {
        path: '',
        children: [
            {
                path: 'dashboard',
                component: AdminDashboardComponent,
                data: {
                    roles: [UserRoleType.Admin]
                }
            },
            {
                path: 'report',
                component: ReportManagementComponent,
                data: {
                    roles: [UserRoleType.Admin]
                }
            }
        ]
    }
]
