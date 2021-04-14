import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { User } from 'oidc-client';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    private _auth: AuthorizeService,
    private _router: Router) { }

  redirects = {
    "0": "/",
    "1": "/",
    "2": "/",
    "3": "/"
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const expectedRole = route.data.expectedRole
    let role = 0;
    this._auth.getAuthLevel().subscribe(num => role = num);
    if (this._auth.isAuthenticated() && expectedRole <= role) {
      return true;
    } else {
      this._router.navigate([this.redirects[role.toString()]]);
      return false;
    }
  }

}
