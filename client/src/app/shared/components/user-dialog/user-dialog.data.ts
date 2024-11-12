import { UserDialogModel } from "../../../core/models/user/user-dialog.model";

export const UserDialogCardData: UserDialogModel[] = [
    {
        iconName: '@tui.baggage-claim',
        title: 'Purchase Orders',
        path: '/user-management'
    },
    {
        iconName: '@tui.file-badge',
        title: 'Sale Orders',
        path: '/user-management'
    }
]

export const UserConvenientData: UserDialogModel[] = [
    {
        iconName: '@tui.heart',
        title: 'Saved Orders',
        path: '/user-management'
    },
    {
        iconName: '@tui.bookmark',
        title: 'Saved Search',
        path: '/user-management'
    }
    ,
    {
        iconName: '@tui.star',
        title: 'My Reviews',
        path: '/user-management'
    }
]

export const UserDialogMoreData: UserDialogModel[] = [
    {
        iconName: '@tui.settings',
        title: 'Account Settings',
        path: '/user-management'
    },
    {
        iconName: '@tui.badge-help',
        title: 'Helps',
        path: '/user-management'
    },
    {
        iconName: '@tui.lightbulb',
        title: 'Contribute ideas',
        path: '/user-management'    
    },
]