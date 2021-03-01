import { BrowserModule } from '@angular/platform-browser';
import { LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule, BsLocaleService } from 'ngx-bootstrap/datepicker'
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { DatePipe } from '@angular/common'
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { ImageCropperModule } from 'ngx-image-cropper';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { DateInputComponent } from './_forms/date-input/date-input.component';

import { AddAnimalComponent } from './animal/add-animal/add-animal.component';
import { EditAnimalComponent } from './animal/edit-animal/edit-animal.component';
import { ListAnimalComponent } from './animal/list-animal/list-animal.component';

import { AddSpeciesComponent } from './species/add-species/add-species.component';
import { ListSpeciesComponent } from './species/list-species/list-species.component';
import { EditSpeciesComponent } from './species/edit-species/edit-species.component';

import { AddTreatmentComponent } from './doctor/treatment/add-treatment/add-treatment.component';
import { EditTreatmentComponent } from './doctor/treatment/edit-treatment/edit-treatment.component';
import { ListOwnTreatmentComponent } from './doctor/treatment/list-own-treatment/list-own-treatment.component';

import { AddTreatmentTimeComponent } from './doctor/treatment/times/add-treatment-time/add-treatment-time.component';
import { EditTreatmentTimeComponent } from './doctor/treatment/times/edit-treatment-time/edit-treatment-time.component';
import { ListTreatmentTimeComponent } from './doctor/treatment/times/list-treatment-time/list-treatment-time.component';

import { BookAppointmentComponent } from './appointment/book-appointment/book-appointment.component';

import { MatModule } from './mat/mat.module';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { huLocale } from 'ngx-bootstrap/locale';
import { MyBookedAppointmentsComponent } from './appointment/my-booked-appointments/my-booked-appointments.component';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { AddHolidayComponent } from './doctor/holiday/add-holiday/add-holiday.component';
import { EditHolidayComponent } from './doctor/holiday/edit-holiday/edit-holiday.component';
import { ListHolidayComponent } from './doctor/holiday/list-holiday/list-holiday.component';
import { AddDoctorComponent } from './doctor/crud/add-doctor/add-doctor.component';
import { ListDoctorComponent } from './doctor/crud/list-doctor/list-doctor.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    AddAnimalComponent,
    TextInputComponent,
    DateInputComponent,
    ListAnimalComponent,
    EditAnimalComponent,
    ListSpeciesComponent,
    AddSpeciesComponent,
    EditSpeciesComponent,
    AddTreatmentComponent,
    EditTreatmentComponent,
    ListOwnTreatmentComponent,
    AddTreatmentTimeComponent,
    EditTreatmentTimeComponent,
    ListTreatmentTimeComponent,
    BookAppointmentComponent,
    MyBookedAppointmentsComponent,
    AddHolidayComponent,
    EditHolidayComponent,
    ListHolidayComponent,
    AddDoctorComponent,
    ListDoctorComponent
  ],
  imports: [
	MatModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    TimepickerModule.forRoot(),
    ApiAuthorizationModule,
    RouterModule.forRoot([
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'counter', component: CounterComponent},
    { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },

    { path: 'appointment', component: BookAppointmentComponent},
    { path: 'appointment/my-appointments', component: MyBookedAppointmentsComponent},

    { path: 'species/list', component: ListSpeciesComponent },

    { path: 'doctors', component: ListDoctorComponent },
    { path: 'holidays', component: ListHolidayComponent},

    { path: 'treatments/my-treatments', component: ListOwnTreatmentComponent },
    { path: 'treatments/my-treatments/:treatmentid', component: ListTreatmentTimeComponent },
    { path: 'treatments/addtreatment', component: AddTreatmentTimeComponent },

    { path: 'animals/myanimals', component: ListAnimalComponent },
    { path: 'animals/edit/:animalid', component: EditAnimalComponent },

    ], { relativeLinkResolution: 'legacy' }),
    NgbModule,
    ImageCropperModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    {provide: MAT_DATE_LOCALE, useValue: 'hu-HU'},
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { constructor(localeService: BsLocaleService) {
  defineLocale('hu', huLocale);
  localeService.use('hu');
}}
