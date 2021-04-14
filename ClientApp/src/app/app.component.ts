import { OnInit } from '@angular/core';
import { Component, OnDestroy } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'app';

  subscription: Subscription;

  constructor(private router: Router, private auth: AuthorizeService) { }

  ngOnInit() {
    this.subscription = this.router.events.subscribe(async (event) => {
      if (event instanceof NavigationStart) {
        if (!this.router.navigated) {
          await this.auth.loadRole();
        }
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
