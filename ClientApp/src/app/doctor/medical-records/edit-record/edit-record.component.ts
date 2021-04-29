import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { AnimalDto } from 'src/app/_models/animaldto';
import { MedicalRecordDto } from 'src/app/_models/MedicalRecordDto';
import { TherapiaDto } from 'src/app/_models/therapiadto';
import { LocalTherapiaRecord, TherapiaOnMedicalRecord } from 'src/app/_models/therapiarecorddto';
import { UpdateMedicalRecordDto } from 'src/app/_models/UpdateMedicalRecordDto';
import { AnimalService } from 'src/app/_services/animal.service';
import { MedicalRecordService } from 'src/app/_services/medical-record.service';
import { TherapiaService } from 'src/app/_services/therapia.service';

@Component({
  selector: 'app-edit-record',
  templateUrl: './edit-record.component.html',
  styleUrls: ['./edit-record.component.css']
})
export class EditRecordComponent implements OnInit {

  validationErrors;
  originalMedicalRecord: MedicalRecordDto;

  uploader: FileUploader;
  hasBaseDropZoneOver = false;

  addRecordForm: FormGroup;
  animals: AnimalDto[] = [];
  therapias: TherapiaDto[] = [];
  therapiaRecords: LocalTherapiaRecord[] = [];

  constructor(private fb: FormBuilder, private animalService: AnimalService, private therapiaService: TherapiaService, private medicalRecordService: MedicalRecordService, private activatedRoute: ActivatedRoute) { }

  therapiaRecordForm: FormGroup;
  filteredTherapias: Observable<TherapiaDto[]>;

  therapiaIndexCount;

  imagesToDelete: number[] = [];
  therapiasToDelete: number[] = [];


  ngOnInit(): void {
    this.therapiaIndexCount = 0;
    this.medicalRecordService.getMedicalRecordById(this.activatedRoute.snapshot.paramMap.get('recordid'))
      .subscribe((med: MedicalRecordDto) => {
        this.originalMedicalRecord = med;
        console.log(this.originalMedicalRecord)
        this.animalService.getUserAnimalsByEmail(med.ownerEmail).subscribe((animals: AnimalDto[]) => {
          this.animals = animals;

          this.addRecordForm = this.fb.group(
            {
              email: [med.ownerEmail, Validators.required],
              animalId: [med.animalId],
              anamnesis: [med.anamnesis],
              symptoma: [med.symptoma],
              details: [med.details]
            }
          )

          med.therapiaRecords.forEach(tr => {
            console.log(tr.id)
            let localRecord: LocalTherapiaRecord = {
              amount: tr.amount,
              id: tr.id,
              dbId: tr.id,
              therapiaId: tr.therapiaId,
              therapiaName: tr.therapiaName
            };
            this.therapiaRecords.push(localRecord);
          })

          this.therapiaRecords.forEach(tr => {
            if (tr.id >= this.therapiaIndexCount) {
              this.therapiaIndexCount = tr.id + 1;
            }
          })

          this.therapiaRecordForm = this.fb.group(
            {
              id: [this.therapiaIndexCount++],
              therapiaId: ['', Validators.required],
              amount: [''],
            }
          )

          this.initializeUploader();
          this.therapiaService.getTherapias().subscribe((therapias: TherapiaDto[]) => {
            this.therapias = therapias;
            this.filteredTherapias = this.therapiaRecordForm.controls['therapiaId'].valueChanges
              .pipe(
                startWith(''),
                map(value => this._filter(value))
              );
          })
        })
      })
  }

  deleteImage(id) {
    if (confirm("Biztosan törölni szeretnéd a képet?")) {
      this.imagesToDelete.push(id);
      this.originalMedicalRecord.photos = this.originalMedicalRecord.photos.filter(p => p.id != id);
    }
  }

  valuechange(value) {
    this.animalService.getUserAnimalsByEmail(value).subscribe((animals: AnimalDto[]) => {
      this.animals = animals;
    })
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
    if (this.therapiaRecords.filter(t => t.id == id)[0].dbId) {
      this.therapiasToDelete.push(id);
    }
    this.therapiaRecords = this.therapiaRecords.filter(t => t.id != id);
    this.therapiaRecords = [...this.therapiaRecords];
  }

  private _filter(value: string) {
    return this.therapias.filter(option => option.name.toLowerCase().includes(value.toString().toLowerCase()));
  }

  post() {

    this.therapiasToDelete.forEach(ttd => {
      this.medicalRecordService.removeTherapiaFromMedicalRecord(ttd).subscribe(res => console.log('therapia del: success (' + ttd + ')'))
    })

    this.imagesToDelete.forEach(itd => {
      this.medicalRecordService.deletePhoto(itd).subscribe(res => console.log('img del: success (' + itd + ')'))
    })


    let therapiasConverted: TherapiaOnMedicalRecord[] = this.therapiaRecords.filter(t => !t.dbId).map(t => ({ therapiaId: t.therapiaId, amount: t.amount }));

    let updatedMedicalRecord: UpdateMedicalRecordDto = {
      id: this.originalMedicalRecord.id,
      date: new Date(this.originalMedicalRecord.date),
      ownerEmail: this.addRecordForm.get('email').value,
      animalId: this.addRecordForm.get('animalId').value === -1 ? null : this.addRecordForm.get('animalId').value,
      anamnesis: this.addRecordForm.get('anamnesis').value,
      symptoma: this.addRecordForm.get('symptoma').value,
      details: this.addRecordForm.get('details').value,
      therapias: therapiasConverted
    };

    this.medicalRecordService.updateMedicalRecord(updatedMedicalRecord).subscribe((record: MedicalRecordDto) => {
      this.uploader.setOptions({ url: 'https://localhost:44345/api/records/add-photo/' + record.id });
      this.uploader.uploadAll();
    }, err => this.validationErrors = err);

  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    let token: any = JSON.parse(sessionStorage.getItem("oidc.user:https://localhost:44345:Vet"));
    this.uploader = new FileUploader({
      url: 'https://localhost:44345/api/records/add-photo',
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
