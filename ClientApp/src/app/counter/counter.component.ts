import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent implements OnInit {
  public currentCount = 0;
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('https://localhost:44345/api/users').subscribe((res: any) => {
      this.currentCount = res.authLevel;
    })
  }

  public incrementCounter() {
    this.currentCount++;
  }

  
}
