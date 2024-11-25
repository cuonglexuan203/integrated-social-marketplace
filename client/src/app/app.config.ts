import { ApplicationConfig, provideZoneChangeDetection, importProvidersFrom } from '@angular/core';
import { provideRouter, withComponentInputBinding, withInMemoryScrolling } from '@angular/router';
import { routes } from './app.routes';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { HTTP_INTERCEPTORS, HttpRequest, provideHttpClient, withInterceptorsFromDi, HttpClient } from '@angular/common/http';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { AuthenticationModule } from './pages/authentication/authentication.module';
import { NbRoleProvider, NbSecurityModule } from '@nebular/security';
import { provideEnvironmentNgxMask } from 'ngx-mask';
import { NB_AUTH_TOKEN_INTERCEPTOR_FILTER, NbAuthJWTInterceptor } from '@nebular/auth';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { environment } from '../environments/endpoint';
import { RoleProvider } from './shared/roles/role.provider';
import { provideHotToastConfig } from '@ngxpert/hot-toast';
import { NG_EVENT_PLUGINS } from '@taiga-ui/event-plugins';
import { provideLottieOptions } from 'ngx-lottie';

// icons
import { BrowserModule } from '@angular/platform-browser';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes,
      withInMemoryScrolling({
        scrollPositionRestoration: 'enabled',
        anchorScrolling: 'enabled',
      }),
      withComponentInputBinding()
    ),
    NG_EVENT_PLUGINS,
    importProvidersFrom(FormsModule,
      ReactiveFormsModule,
      NgScrollbarModule,
      AuthenticationModule,
      NbSecurityModule.forRoot()
    ),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptorsFromDi()),
    provideHotToastConfig(),
    BrowserModule,
    {
      provide: HTTP_INTERCEPTORS, useClass: NbAuthJWTInterceptor, multi: true
    },
    // {
    //   provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true
    // },
    {
      provide: NB_AUTH_TOKEN_INTERCEPTOR_FILTER, useValue: (req: HttpRequest<any>) => {
        if (req.url.includes(`${environment.apiAuth}/login`)) {
          return true;
        }
        return false;
      }
    },
    { provide: NbRoleProvider, useClass: RoleProvider },
    provideEnvironmentNgxMask(),

    provideLottieOptions({
      player: () => import('lottie-web'),
    }),
  ]
};
