import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router, NavigationExtras } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {
                const errors = {};
                for (let key in error.error.errors) {
                  errors[this.camelize(key)] = error.error.errors[key];
                }
                throw errors;
              } else {
                this.toastr.error(error.statusText == "OK" ? error.error.title : error.statusText, "Sikertelen művelet");
              }
              break;
            case 401:
              this.toastr.error(error.statusText == "OK" ? "Nincs jogosultságod a művelet végrehajtásához." : error.statusText, "Sikertelen művelet");
              break;
            case 404:
              this.toastr.warning(error.error.title)
              break;
            case 500:
            /* const navigationExtras: NavigationExtras = { state: { error: error.error } }
             this.router.navigateByUrl('/server-error', navigationExtras);
             break;*/
            default:
              this.toastr.error('Valami hiba történt.');
              console.log(error);
              break;
          }
        }
        return throwError(error);
      })
    )
  }

  camelize(str): string {
    return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function (word, index) {
      return index === 0 ? word.toLowerCase() : word.toUpperCase();
    }).replace(/\s+/g, '');
  }


}