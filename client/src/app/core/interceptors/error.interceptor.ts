import { Injectable, Injector } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap, takeUntil } from 'rxjs/operators';
import { Router, ActivationEnd } from '@angular/router';
import { AlertService } from '../services/alert/alert.service';
import { CancelRequestService } from '../services/cancel-request.service';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private inj: Injector,
    private alert: AlertService,
    private router: Router,
    private cancelRequestService: CancelRequestService

  ) {
    router.events.subscribe(event => {
      // An event triggered at the end of the activation part of the Resolve phase of routing.
      if (event instanceof ActivationEnd) {
        // Cancel pending calls
        // this.cancelRequestService.cancelPendingRequests();
      }
    });
  }
  // tslint:disable-next-line: max-line-length
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Clone the request and add a custom header
    const customReq = req.clone({ headers: req.headers.set('CurrentRoute', window.location.pathname) });

    return next.handle(customReq).pipe(
      // Cancel pending requests when the component is destroyed
      takeUntil(this.cancelRequestService.onCancelPendingRequests()),
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          // Handle successful responses with custom messages
          if (event?.body?.message) {
            this.alert.showError(event.body.message, 'Error');
          }
        }
      }),
      catchError((error: HttpErrorResponse) => {
        console.error(error);
        if (error.status === 401 || error.status === 0 || error.status === 400) {
          if (error.status === 401) {
            this.router.navigate(['/login']);
            this.alert.showError(error.status == 401 ? 'Your Username or Password is incorrect. Please try again' : 'Sorry, the system has an unexpected technical issue.', 'Error');
            return throwError(error?.statusText);
          }
           else {
            this.alert.showError('Sorry, the system has an unexpected technical issue.', 'Error');
            return throwError(error?.statusText);
          }
        }
        else if (error.status === 404) {
          this.alert.showError('The requested resource could not be found. Please try again later.', 'Error');
        } else if (error.status === 302) {
          this.router.navigate([`/expired`]);
        } else if (error.status === 400 && error?.error) {
          this.alert.showError('Your Username or Password is incorrect. Please try again', 'Error');
        }
        return throwError(error?.statusText);
      }));
  }
}
