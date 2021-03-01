import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatVerticalStepper } from '@angular/material/stepper';
import { AppointmentTimeDto } from '../_models/appointmenttimedto';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent implements OnInit {
  @ViewChild(MatVerticalStepper)
  stepper: MatVerticalStepper;

  public currentCount = 0;
  constructor(private http: HttpClient) {}
  dd: AppointmentTimeDto[];
  ngOnInit(): void {
    let params = new HttpParams()
    .set('time', new Date().toISOString())
    .set('doctorId', '5addde3b-8ce0-4914-982d-8fd8f6aa7009');
    this.http.get('https://localhost:44345/api/appointments/my-appointments').subscribe((res : any) => {
      console.log(res);
    })
  }

  public incrementCounter() {
    this.currentCount++;
  }

  
}
