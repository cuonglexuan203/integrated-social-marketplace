import { Routes } from '@angular/router';
import { AccountSettingComponent } from './account-setting/account-setting.component';
import { ProfileComponent } from './account-setting/profile/profile.component';
import { SecurityComponent } from './account-setting/security/security.component';
import { SocialMediaComponent } from './account-setting/social-media/social-media.component';

export const UserManagementRoutes: Routes = [
    {
        path: 'account-settings',
        component: AccountSettingComponent,
        data: {
            title: 'Account Settings',
        },
        children: [
            {
                path: 'profile',
                component: ProfileComponent,
                data: {
                    title: 'Profile',
                }
            },
            {
                path: 'security',
                component: SecurityComponent,
                data: {
                    title: 'Security',
                }
            },
            {
                path: 'social-media',
                component: SocialMediaComponent,
                data: {
                    title: 'Social Media',
                }
            }
        ]
    }
]