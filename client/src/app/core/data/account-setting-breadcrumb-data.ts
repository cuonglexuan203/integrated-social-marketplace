import { BreadcrumbModel } from "../models/breadscrumb/breadcrumb.model";

export const AccountSettingBreadcrumbData: BreadcrumbModel[] = [
    {
        routerLink: '/home',
        caption: 'Home',
    },
    {
        routerLink: '/user/account-settings/profile',
        caption: 'Account Settings',
        currentColor: 'blue',
    },
]