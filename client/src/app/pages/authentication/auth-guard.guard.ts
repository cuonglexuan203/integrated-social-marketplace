import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot } from '@angular/router';
import { NbAuthService } from '@nebular/auth';
import { NbAccessChecker, NbAclService } from '@nebular/security';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: NbAuthService, private router: Router, private access: NbAccessChecker,
  ) {

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.isAuthenticated().pipe(
      map(isAuthenticated => {
        if (!isAuthenticated) {
          this.router.navigate(['/login']);
          return false;
        }
        return true;
      })
    );
  }
}

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivateChild {
  constructor(
    private authService: NbAuthService,
    private access: NbAccessChecker,
    private router: Router
  ) { }
  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot,) {
    var data = childRoute.data['roles'] as string[];
    if (data) {
      return this.authService.getToken().pipe(
        map(e => {
          if (!e.isValid()) {
            this.router.navigate(['/login']);
            return false;
          }
          else {
            var role = e.getPayload()['role'];
            let roleArray = Array.isArray(role) ? role : [role];
            const isValid = data.some(element => roleArray.includes(element));
            if (isValid) return true;
            else {
              this.router.navigate(['/error']);
              return false;
            }
          }
        })
      );
    }
    else return true;
  }
}
