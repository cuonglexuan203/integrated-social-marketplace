import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateChild, GuardResult, MaybeAsync, RouterStateSnapshot } from "@angular/router";
import { NbAuthService, NbAuthToken } from "@nebular/auth";
import { NbAccessChecker, NbRoleProvider } from "@nebular/security";
import { Observable, map } from "rxjs";

@Injectable()
export class RoleProvider implements NbRoleProvider {

  constructor(private authService: NbAuthService) {
  }

  getRole(): Observable<string> {
    return this.authService.getToken()
      .pipe(
        map((token: NbAuthToken) => {
          return token.isValid() ? token.getPayload()['role'] : 'guest';
        }),
      );
  }
}
