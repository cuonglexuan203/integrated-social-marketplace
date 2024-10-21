import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ChatComponent } from './chat/chat.component';

export const PagesRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
        data: {
            title: 'Home'
        }
    },
]