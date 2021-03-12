import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { EditAnimalComponent } from '../animal/edit-animal/edit-animal.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(component: EditAnimalComponent): boolean {
    if(component.editAnimalForm.dirty){
      return confirm('Nem mentett változtatásaid vannak. Biztosan el szeretnéd hagyni az oldalt?');
    }
    return true;
  }
  
}
