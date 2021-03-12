import { Component } from '@angular/core';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

const mycolor = "#6A82CA";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;


  constructor(public auth: AuthorizeService) { }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
