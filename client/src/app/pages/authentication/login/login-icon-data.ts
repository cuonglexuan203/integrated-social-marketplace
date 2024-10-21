export class LogoLoginModel {
    name: string;
    iconName: string;
    width: number;
    height: number;
    color: string;
}

export const LOGO_LOGIN_LEFT_SIDE: LogoLoginModel[] = [
    {
        name: 'Buy & Sell',
        iconName: '@tui.badge-dollar-sign',
        width: 24,
        height: 24,
        color: '#ff8c00'
    },
    {
        name: 'Chat',
        iconName: '@tui.message-circle',
        width: 24,
        height: 24,
        color: '#1e90ff'
    },
    {
        name: 'AI Assistant',
        iconName: '@tui.bot',
        width: 24,
        height: 24,
        color: '#ff69b4'
    },
]

export const LOGO_LOGIN_RIGHT_SIDE : LogoLoginModel[] = [
    {
        name: 'Login with Facebook',
        iconName: '@tui.facebook',
        width: 24,
        height: 24,
        color: '#3b5998'
    },
    {
        name: 'Login with Google',
        iconName: '@tui.chrome',
        width: 24,
        height: 24,
        color: '#db4437'
    },
    {
        name: 'Login with Twitter',
        iconName: '@tui.twitter',
        width: 24,
        height: 24,
        color: '#1da1f2'
    },
]