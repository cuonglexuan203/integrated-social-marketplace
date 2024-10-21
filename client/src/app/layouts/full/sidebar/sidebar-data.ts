import { NavItem } from "./nav-item/nav-item";

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
        displayName: 'User',
        iconName: '@tui.contact',
        bgColor: 'white',
        route: '/user',
        tooltip: 'User'
    },
    {
        displayName: 'Chat',
        iconName: '@tui.message-circle-more',
        bgColor: 'white',
        route: '/chat',
        tooltip: 'Chat'
    },
    {
        displayName: 'Create Post',
        iconName: '@tui.plus',
        bgColor: 'white',
        route: '/create-post',
        tooltip: 'Create Post'
    },
    {
        displayName: 'Saved Posts',
        iconName: '@tui.file-heart',
        bgColor: 'white',
        route: '/saved-posts',
        tooltip: 'Saved Posts'
    },
    {
        displayName: 'Cart',
        iconName: '@tui.shopping-cart',
        bgColor: 'white',
        route: '/cart',
        tooltip: 'Cart'
    }

]