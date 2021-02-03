import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
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
import { AddAnimalComponent } from './animal/add-animal/add-animal.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { DateInputComponent } from './_forms/date-input/date-input.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ListAnimalComponent } from './animal/list-animal/list-animal.component';

import { ImageCropperModule } from 'ngx-image-cropper';
import { EditAnimalComponent } from './animal/edit-animal/edit-animal.component';
import { ListSpeciesComponent } from './species/list-species/list-species.component';
import { AddSpeciesComponent } from './species/add-species/add-species.component';
import { EditSpeciesComponent } from './species/edit-species/edit-species.component';
import { AddTreatmentComponent } from './treatment/add-treatment/add-treatment.component';
import { EditTreatmentComponent } from './treatment/edit-treatment/edit-treatment.component';
import { ListOwnTreatmentComponent } from './treatment/list-own-treatment/list-own-treatment.component';
import { AddTreatmentTimeComponent } from './treatment/times/add-treatment-time/add-treatment-time.component';
import { EditTreatmentTimeComponent } from './treatment/times/edit-treatment-time/edit-treatment-time.component';
import { ListTreatmentTimeComponent } from './treatment/times/list-treatment-time/list-treatment-time.component';

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
    ListTreatmentTimeComponent
  ],
  imports: [
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
    { path: 'animals/myanimals', component: ListAnimalComponent },
    { path: 'species/list', component: ListSpeciesComponent },
    { path: 'treatments/my-treatments', component: ListOwnTreatmentComponent },
    { path: 'treatments/my-treatments/:treatmentid', component: ListTreatmentTimeComponent },
    { path: 'treatments/addtreatment', component: AddTreatmentTimeComponent },
    { path: 'animals/edit/:animalid', component: EditAnimalComponent },
    { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
    ], { relativeLinkResolution: 'legacy' }),
    NgbModule,
    ImageCropperModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
