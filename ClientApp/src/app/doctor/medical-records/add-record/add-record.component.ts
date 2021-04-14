import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { AnimalDto } from 'src/app/_models/animaldto';
import { AnimalService } from 'src/app/_services/animal.service';
import { startWith, map } from 'rxjs/operators';
import { TherapiaDto } from 'src/app/_models/therapiadto';
import { TherapiaService } from 'src/app/_services/therapia.service';
import { LocalTherapiaRecord, TherapiaOnMedicalRecord } from 'src/app/_models/therapiarecorddto';
import { AddMedicalRecordDto } from 'src/app/_models/AddMedicalRecordDto';
import { MedicalRecordService } from 'src/app/_services/medical-record.service';
import { FileUploader } from 'ng2-file-upload';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { VetUserDto } from 'src/app/_models/vetuserdto';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add-record',
  templateUrl: './add-record.component.html',
  styleUrls: ['./add-record.component.css']
})
export class AddRecordComponent implements OnInit {

  uploader: FileUploader;
  hasBaseDropZoneOver = false;

  addRecordForm: FormGroup;
  animals: AnimalDto[] = [];
  therapias: TherapiaDto[] = [];
  therapiaRecords: LocalTherapiaRecord[] = [];

  constructor(private fb: FormBuilder, private animalService: AnimalService,
    private therapiaService: TherapiaService, private medicalRecordService: MedicalRecordService,
    private route: ActivatedRoute, private userService: UserService) { }

  therapiaRecordForm: FormGroup;
  filteredTherapias: Observable<TherapiaDto[]>;

  therapiaIndexCount = 0;


  ngOnInit(): void {

    this.initializeUploader();
    this.therapiaService.getTherapias().subscribe((therapias: TherapiaDto[]) => {
      this.therapias = therapias;
      this.filteredTherapias = this.therapiaRecordForm.controls['therapiaId'].valueChanges
        .pipe(
          startWith(''),
          map(value => this._filter(value))
        );
    })

    this.therapiaRecordForm = this.fb.group(
      {
        id: [this.therapiaIndexCount++],
        therapiaId: ['', Validators.required],
        amount: [''],
      }
    )

    this.addRecordForm = this.fb.group(
      {
        email: ['', Validators.required],
        animalId: [-1],
        anamnesis: [''],
        symptoma: [''],
        details: ['']
      }
    )

    this.route.params.subscribe(params => {
      if (params['userId']) {
        this.userService.getUserById(params['userId']).subscribe((user: VetUserDto) => {
          this.addRecordForm.patchValue({ email: user.email });
          this.animalService.getUserAnimalsByEmail(user.email).subscribe((animals: AnimalDto[]) => {
            this.animals = animals;
            if (params['animalId']) {
              if (!isNaN(params['animalId'])) {
                if (this.animals.filter(a => a.id === Number(params['animalId'])).length > 0) {
                  this.addRecordForm.patchValue({ animalId: Number(params['animalId']) });
                }
              }
            }
          })
        })
      }

    })
  }

  valuechange(value) {
    this.animalService.getUserAnimalsByEmail(value).subscribe((animals: AnimalDto[]) => {
      this.animals = animals;
    })
  }

  getName(therapiaId: number) {
    return this.therapias.find(therapia => therapia.id === therapiaId)?.name;
  }

  addTherapiaToRecord() {
    let newTherapiaRecord: LocalTherapiaRecord = {
      id: this.therapiaRecordForm.get('id').value,
      therapiaName: this.therapias.find(t => t.id == this.therapiaRecordForm.get('therapiaId').value).name,
      therapiaId: this.therapiaRecordForm.get('therapiaId').value,
      amount: this.therapiaRecordForm.get('amount').value
    }

    this.therapiaRecords.push(newTherapiaRecord);
    this.therapiaRecords = [...this.therapiaRecords];

    this.therapiaRecordForm = this.fb.group(
      {
        id: [this.therapiaIndexCount++],
        therapiaId: ['', Validators.required],
        amount: ['', Validators.required],
      }
    )

    this.therapiaRecordForm.patchValue({ id: this.therapiaIndexCount++ })
  }

  removeTherapiaFromRecord(id) {
    this.therapiaRecords = this.therapiaRecords.filter(t => t.id != id);
    this.therapiaRecords = [...this.therapiaRecords];
  }

  private _filter(value: string) {
    return this.therapias.filter(option => option.name.toLowerCase().includes(value.toString().toLowerCase()));
  }

  post() {
    let therapiasConverted: TherapiaOnMedicalRecord[] = this.therapiaRecords.map(t => ({ therapiaId: t.therapiaId, amount: t.amount }));
    let newMedicalRecord: AddMedicalRecordDto = {
      date: new Date(),
      ownerEmail: this.addRecordForm.get('email').value,
      animalId: this.addRecordForm.get('animalId').value === -1 ? null : this.addRecordForm.get('animalId').value,
      anamnesis: this.addRecordForm.get('anamnesis').value,
      symptoma: this.addRecordForm.get('symptoma').value,
      details: this.addRecordForm.get('details').value,
      therapias: therapiasConverted
    };


    this.medicalRecordService.addMedicalRecord(newMedicalRecord).subscribe(id => {
      this.uploader.setOptions({ url: environment.apiUrl + 'records/add-photo/' + id });
      this.uploader.uploadAll();
    });
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    let token: any = JSON.parse(sessionStorage.getItem("oidc.user:https://localhost:44345:Vet"));
    this.uploader = new FileUploader({
      url: environment.apiUrl + 'records/add-photo/',
      authToken: 'Bearer ' + token.access_token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        console.log(response);
      }
    }
  }
}
