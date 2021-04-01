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
import { RoleGuard } from './_guards/role.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MAT_RADIO_DEFAULT_OPTIONS } from '@angular/material/radio';
import { AddTherapiaComponent } from './admin/therapias/add-therapia/add-therapia.component';
import { EditTherapiaComponent } from './admin/therapias/edit-therapia/edit-therapia.component';
import { ListTherapiasComponent } from './admin/therapias/list-therapias/list-therapias.component';
import { ListRecordsComponent } from './animal/medical-records/list-records/list-records.component';
import { AddRecordComponent } from './doctor/medical-records/add-record/add-record.component';


import { FileUploadModule } from "ng2-file-upload";
import { EnlargeImageModalComponent } from './animal/medical-records/enlarge-image-modal/enlarge-image-modal.component';
import { EditRecordComponent } from './doctor/medical-records/edit-record/edit-record.component';
import { ListAllRecordsComponent } from './animal/medical-records/list-all-records/list-all-records.component';
import { VisitorHomepageComponent } from './homepage/visitor-homepage/visitor-homepage.component';
import { UserHomepageComponent } from './homepage/user-homepage/user-homepage.component';
import { DoctorHomepageComponent } from './homepage/doctor-homepage/doctor-homepage.component';
import { AddVaccineComponent } from './admin/vaccines/add-vaccine/add-vaccine.component';
import { EditVaccineComponent } from './admin/vaccines/edit-vaccine/edit-vaccine.component';
import { ListVaccinesComponent } from './admin/vaccines/list-vaccines/list-vaccines.component';
import { ListVaccineRecordsComponent } from './animal/vaccine-records/list-vaccine-records/list-vaccine-records.component';
import { AddVaccineRecordComponent } from './animal/vaccine-records/add-vaccine-record/add-vaccine-record.component';
import { EditVaccineRecordComponent } from './animal/vaccine-records/edit-vaccine-record/edit-vaccine-record.component';
import { ListAnimalByOwnerComponent } from './animal/list-animal-by-owner/list-animal-by-owner.component';
import { ListArchivedAnimalsComponent } from './animal/list-archived-animals/list-archived-animals.component';
import { ListArchivedAnimalByOwnerComponent } from './animal/list-archived-animal-by-owner/list-archived-animal-by-owner.component';
import { ListUsersComponent } from './admin/list-users/list-users.component';

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
    ListDoctorComponent,
    AddTherapiaComponent,
    EditTherapiaComponent,
    ListTherapiasComponent,
    ListRecordsComponent,
    AddRecordComponent,
    EnlargeImageModalComponent,
    EditRecordComponent,
    ListAllRecordsComponent,
    VisitorHomepageComponent,
    UserHomepageComponent,
    DoctorHomepageComponent,
    AddVaccineComponent,
    EditVaccineComponent,
    ListVaccinesComponent,
    ListVaccineRecordsComponent,
    AddVaccineRecordComponent,
    EditVaccineRecordComponent,
    ListAnimalByOwnerComponent,
    ListArchivedAnimalsComponent,
    ListArchivedAnimalByOwnerComponent,
    ListUsersComponent
  ],
  imports: [FileUploadModule,
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
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },

      { path: 'users', component: ListUsersComponent },
      { path: 'animals/myanimals', component: ListAnimalComponent },
      { path: 'animals/myarchivedanimals', component: ListArchivedAnimalsComponent },
      { path: 'animals/edit/:animalid', component: EditAnimalComponent, canDeactivate: [PreventUnsavedChangesGuard] },
      { path: 'animals/records/:animalid', component: ListRecordsComponent },
      { path: 'animals/user/:userid', component: ListAnimalByOwnerComponent },
      { path: 'animals/archived-by-user/:userid', component: ListArchivedAnimalByOwnerComponent },

      { path: 'appointment', component: BookAppointmentComponent },
      { path: 'appointment/my-appointments', component: MyBookedAppointmentsComponent },

      ////////////////////////////////////////////////////////////////////////////////////////
      {
        path: 'holidays', component: ListHolidayComponent,
        canActivate: [RoleGuard],
        data: {
          expectedRole: 2
        }
      },
      {
        path: 'treatments/my-treatments', component: ListOwnTreatmentComponent,
        canActivate: [RoleGuard],
        data: {
          expectedRole: 2
        }
      },
      {
        path: 'treatments/my-treatments/:treatmentid', component: ListTreatmentTimeComponent,
        canActivate: [RoleGuard],
        data: {
          expectedRole: 2
        }
      },
      {
        path: 'treatments/addtreatment', component: AddTreatmentTimeComponent,
        canActivate: [RoleGuard],
        data: {
          expectedRole: 2
        }
      },
      ////////////////////////////////////////////////////////////////////////////////////////
      {
        path: 'species/list', component: ListSpeciesComponent,
        canActivate: [RoleGuard],
        data: {
          expectedRole: 3
        }
      },

      {
        path: 'doctors', component: ListDoctorComponent,
        canActivate: [RoleGuard],
        data: {
          expectedRole: 3
        }
      },

      {
        path: 'record', component: AddRecordComponent,
        /*canActivate: [RoleGuard],
        data: {
          expectedRole: 2
        }*/
      },

      {
        path: 'record/:recordid/edit', component: EditRecordComponent,
        /*canActivate: [RoleGuard],
        data: {
          expectedRole: 2
        }*/
      },

      { path: 'records/my-records', component: ListAllRecordsComponent },

      {
        path: 'therapias', component: ListTherapiasComponent,
        canActivate: [RoleGuard],
        data: {
          expectedRole: 3
        }
      },

      {
        path: 'vaccines', component: ListVaccinesComponent,
        canActivate: [RoleGuard],
        data: {
          expectedRole: 3
        }
      },

    ], { relativeLinkResolution: 'legacy' }),
    NgbModule,
    ImageCropperModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    { provide: MAT_DATE_LOCALE, useValue: 'hu-HU' },
    { provide: MAT_RADIO_DEFAULT_OPTIONS, useValue: { color: 'primary' } },
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(localeService: BsLocaleService) {
    defineLocale('hu', huLocale);
    localeService.use('hu');
  }
}
