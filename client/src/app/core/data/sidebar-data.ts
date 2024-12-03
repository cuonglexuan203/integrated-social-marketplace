import { NavItem } from "../../layouts/full/sidebar/nav-item/nav-item";

export const navItems: NavItem[] = [
    {
        navCap: 'Quick Features'
    },
    {
        displayName: 'Home',
        iconName: '@tui.house',
        bgColor: 'white',
        route: '/home',
        tooltip: 'Home'
    },
    {
        displayName: 'User Profile',
        iconName: '@tui.contact',
        bgColor: 'white',
        route: 'user/user-profile/:userName',
        tooltip: 'User Profile'
    },
    {
        displayName: 'Chat',
        iconName: '@tui.message-circle-more',
        bgColor: 'white',
        route: '/chat',
        tooltip: 'Chat'
    },
    {
        displayName: 'Saved Posts',
        iconName: '@tui.file-heart',
        bgColor: 'white',
        route: '/saved-posts',
        tooltip: 'Saved Posts'
    },
]

export const navItemsAdmin: NavItem[] = [
    {
        navCap: 'Quick Features'
    },
    {
        displayName: 'Home',
        iconName: '@tui.house',
        bgColor: 'white',
        route: '/home',
        tooltip: 'Home'
    },
    {
        displayName: 'User Profile',
        iconName: '@tui.contact',
        bgColor: 'white',
        route: 'user/user-profile/:userName',
        tooltip: 'User Profile'
    },
    {
        displayName: 'Chat',
        iconName: '@tui.message-circle-more',
        bgColor: 'white',
        route: '/chat',
        tooltip: 'Chat'
    },
    {
        displayName: 'Saved Posts',
        iconName: '@tui.file-heart',
        bgColor: 'white',
        route: '/saved-posts',
        tooltip: 'Saved Posts'
    },
    {
        displayName: 'Dashboard',
        iconName: '@tui.layout-dashboard',
        bgColor: 'white',
        route: '/admin/dashboard',
        tooltip: 'Dashboard'
    },
    {
        displayName: 'Report',
        iconName: '@tui.message-square-warning',
        bgColor: 'white',
        route: '/admin/report',
        tooltip: 'Report'
    },
]