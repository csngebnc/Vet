import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatCalendar } from '@angular/material/datepicker';
import { MatVerticalStepper } from '@angular/material/stepper';
import { Router } from '@angular/router';
import { AnimalDto } from 'src/app/_models/animaldto';
import { AppointmentTimeDto } from 'src/app/_models/appointmenttimedto';
import { DoctorDto } from 'src/app/_models/doctordto';
import { HolidayDto } from 'src/app/_models/holidaydto';
import { TreatmentDto } from 'src/app/_models/treatmentdto';
import { TreatmentTimeDto } from 'src/app/_models/treatmenttimedto';
import { AnimalService } from 'src/app/_services/animal.service';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { DoctorService } from 'src/app/_services/doctor.service';
import { TreatmentService } from 'src/app/_services/treatment.service';
import { TreatmenttimeService } from 'src/app/_services/treatmenttime.service';

@Component({
  selector: 'app-book-appointment',
  templateUrl: './book-appointment.component.html',
  styleUrls: ['./book-appointment.component.css']
})
export class BookAppointmentComponent implements OnInit {
  @ViewChild(MatVerticalStepper)
  stepper: MatVerticalStepper;

  @ViewChild(MatCalendar) calendar: MatCalendar<Date>;

  selectDoctorForm: FormGroup;
  selectTreatmentForm: FormGroup;
  selectDateForm: FormGroup;
  selectAnimalForm: FormGroup;
  addAppointmentForm: FormGroup;
  selectedIndex = 0;

  selectedTime = ''

  selectedDate = new Date();
  startAt = new Date();
  minDate = new Date();
  maxDate = new Date();

  animals: AnimalDto[] = [];
  doctors: DoctorDto[] = []

  treatments: TreatmentDto[];
  treatmentTimes: TreatmentTimeDto[];

  selectedDoctorName: string = '';
  selectedTreatmentName = '';
  selectedAnimalName = '';

  avaibleDates: Date[] = [];

  dd: AppointmentTimeDto[] = [];

  doctorHolidays: HolidayDto[] = [];
  doctorHolidayDates: Date[] = [];

  dates: any = [];
  disabledDays: number[] = [0, 1, 2, 3, 4, 5, 6];

  maxAhead: number = 30;

  constructor(private _formBuilder: FormBuilder, private http: HttpClient,
    private treatmentService: TreatmentService, private treatmentTimeService: TreatmenttimeService,
    private animalService: AnimalService, private appointmentService: AppointmentService, private doctorService: DoctorService, private router: Router) {
    this.http.get('https://localhost:44345/api/doctors').subscribe((res: DoctorDto[]) => {
      this.doctors = res;
    });

    this.maxDate.setDate(this.maxDate.getDate() + this.maxAhead);

    let now = new Date(Date.now());
    let today = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0, 0);
    this.avaibleDates.push(new Date(today));
    for (let i = 0; i < this.maxAhead; i++){
      today.setDate(today.getDate()+1)
      this.avaibleDates.push(new Date(today));
    }

  }

  myDateFilter = (d: Date): boolean => {
    return this.avaibleDates.filter(day =>
      day.getFullYear() == d.getFullYear() &&
      day.getMonth() == d.getMonth() &&
      day.getDate() == d.getDate()).length > 0;
  }

  ngOnInit() {
    this.animalService.getUserAnimals().subscribe((animals: AnimalDto[]) => {
      this.animals = animals;
    })

    this.selectDoctorForm = this._formBuilder.group({
      doctorId: ['', Validators.required]
    });

    this.selectTreatmentForm = this._formBuilder.group({
      treatmentId: ['', Validators.required]
    });

    this.selectDateForm = this._formBuilder.group({
      startDate: ['', Validators.required],
      endDate: ['', Validators.required]
    });

    this.selectAnimalForm = this._formBuilder.group({
      animalId: ['']
    });

  }

  loadTreatments() {
    this.selectedDoctorName = (this.doctors.filter(d => d.id == this.selectDoctorForm.get('doctorId').value))[0].realName;

    this.treatmentService.getTreatmentByDoctorId(this.selectDoctorForm.get('doctorId').value).subscribe((res: TreatmentDto[]) => {
      this.doctorService.getDoctorHolidays(this.selectDoctorForm.get('doctorId').value).subscribe((holidays: HolidayDto[]) => {
        this.doctorHolidays = holidays;
        this.doctorHolidays.forEach(h => {
          h.startDate = new Date(h.startDate);
          h.endDate = new Date(h.endDate);
          let current = new Date(h.startDate);
          while (current <= h.endDate) {
            this.doctorHolidayDates.push(new Date(current));
            current.setDate(current.getDate() + 1);
          }
        })
        this.treatments = res;
        this.stepper.next()
      })
    })
  }

  loadTreatmentTimes() {
    this.selectedTreatmentName = (this.treatments.filter(d => d.id == this.selectTreatmentForm.get('treatmentId').value))[0].name;

    this.treatmentTimeService.getTreatmentTimeByTreatmentId(this.selectTreatmentForm.get('treatmentId').value).subscribe((res: TreatmentTimeDto[]) => {
      this.treatmentTimes = res;
      this.treatmentTimes.forEach(time => {
        const index = this.disabledDays.indexOf(time.dayOfWeek, 0);
        if (index > -1)
          this.disabledDays.splice(index, 1);
      });

      this.avaibleDates = this.avaibleDates.filter(d => this.disabledDays.indexOf(d.getDay()) < 0);

      this.avaibleDates = this.avaibleDates.filter(d =>
        this.doctorHolidayDates.filter(hd =>
          hd.getFullYear() == d.getFullYear() &&
          hd.getMonth() == d.getMonth() &&
          hd.getDate() == d.getDate()
        ).length <= 0
      );

      this.startAt = this.avaibleDates[0];
      this.calendar.selected = this.avaibleDates[0];
      this.calendar.activeDate = this.avaibleDates[0];
      this.calendar.updateTodaysDate();
      this.loadReservedTimes(this.avaibleDates[0]);
      this.stepper.next()
    })
  }

  createTimes(date: Date) {
    const weekDay = date.getDay();
    let index = 0;
    const now = new Date(Date.now());
    now.setMinutes(now.getMinutes() + 10);
    if (this.treatmentTimes) {
      this.treatmentTimes.forEach(time => {
        if (time.dayOfWeek == weekDay) {
          let min = new Date(date.getFullYear(), date.getMonth(), date.getDate(), time.startHour, time.startMin, 0, 0);
          let max = new Date(date.getFullYear(), date.getMonth(), date.getDate(), time.endHour, time.endMin, 0, 0);
          let start = new Date(min);
          while (start < max) {
            let add = { startDate: new Date(start), endDate: new Date(start.setMinutes(start.getMinutes() + time.duration)), id: this.dates.length }
            let ok = true;
            if(add.startDate<now) ok = false;
            for (let i = 0; i < this.dd.length; i++) {
              if ((this.dd[i].startDate <= add.startDate && this.dd[i].endDate > add.startDate) || (this.dd[i].startDate >= add.startDate && this.dd[i].endDate < add.endDate) ||
                (this.dd[i].startDate < add.endDate && this.dd[i].endDate > add.endDate) || (this.dd[i].startDate <= add.startDate && this.dd[i].endDate > add.endDate) ||
                (add.startDate<now)) 
                {
                ok = false;
                break;
              }
            }
            if (ok) { this.dates.push(add) }
          }
        }
      });
    }
  }

  onValueChange(value: Date) {
    this.selectDateForm = this._formBuilder.group({
      startDate: ['', Validators.required],
      endDate: ['', Validators.required]
    });
    this.selectedTime = ''

    this.calendar.selected = value;
    this.calendar.updateTodaysDate();
    this.loadReservedTimes(value);
  }

  loadReservedTimes(value: Date) {
    let params = new HttpParams()
      .set('time', value.toLocaleString())
      .set('doctorId', this.selectDoctorForm.get('doctorId').value);

    this.dates = [];
    this.http.get('https://localhost:44345/api/appointments/getreserved', { params }).subscribe((res: AppointmentTimeDto[]) => { // SERVICE!!!!!!!!
      this.dd = res;
      this.dd.forEach(d => {
        d.startDate = new Date(d.startDate);
        d.endDate = new Date(d.endDate);
      })
      this.createTimes(value);
    });
  }



  setSelectedTime(id) {
    this.selectedTime = id;
    this.selectDateForm.patchValue({ startDate: this.dates[id].startDate, endDate: this.dates[id].endDate });
    this.stepper.next();
  }

  createFinalForm() {
    this.selectedAnimalName = (this.animals.filter(d => d.id == this.selectAnimalForm.get('animalId').value))[0]?.name;
    this.addAppointmentForm = this._formBuilder.group({
      startDate: [this.selectDateForm.get('startDate').value, Validators.required],
      endDate: [this.selectDateForm.get('endDate').value, Validators.required],
      treatmentId: [this.selectTreatmentForm.get('treatmentId').value, Validators.required],
      doctorId: [this.selectDoctorForm.get('doctorId').value, Validators.required],
      animalId: [this.selectAnimalForm.get('animalId').value === '' ? null : this.selectAnimalForm.get('animalId').value]
    })
  }

  bookAppointment() {
    this.appointmentService.bookAppointment(this.addAppointmentForm.value).subscribe(res => {
      alert("Sikeres időpontfoglalás!")
      this.router.navigateByUrl('/');
    })
  }

  setEmpty() {
    this.selectAnimalForm = this._formBuilder.group({
      animalId: ['']
    });
    this.selectedAnimalName = '';
  }
}
