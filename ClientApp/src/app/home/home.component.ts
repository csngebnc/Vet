import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  constructor(public auth: AuthorizeService, private http: HttpClient) {
  }

  ngOnInit(): void {
  }
}
