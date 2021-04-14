import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AnimalService } from 'src/app/_services/animal.service';
import { DatePipe } from '@angular/common'
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ImageCroppedEvent, base64ToFile } from 'ngx-image-cropper';
import { SpeciesService } from 'src/app/_services/species.service';
import { AnimalDto } from 'src/app/_models/animaldto';

@Component({
  selector: 'app-add-animal',
  templateUrl: './add-animal.component.html',
  styleUrls: ['./add-animal.component.css']
})
export class AddAnimalComponent implements OnInit {

  imageChangedEvent: any = '';
  addAnimalForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];
  speciesList: any;

  constructor(private animalService: AnimalService, private speciesService: SpeciesService, private fb: FormBuilder, public datepipe: DatePipe, private ngbModal: NgbActiveModal) { }

  ngOnInit(): void {
    this.speciesService.getAnimalSpecies().subscribe(res => {
      this.speciesList = res;
      this.initializeForm();
    })
    this.maxDate = new Date();
  }

  initializeForm() {
    this.addAnimalForm = this.fb.group({
      name: ['', Validators.required],
      sex: ['hím', Validators.required],
      dateOfBirth: ['', Validators.required],
      speciesid: ['', Validators.required],
      photo: ['']
    })
  }

  addAnimal(){
    let formData = new FormData();
    formData.append('name',this.addAnimalForm.get('name').value);
    formData.append('sex',this.addAnimalForm.get('sex').value);
    formData.append('dateOfBirth',this.datepipe.transform(this.addAnimalForm.get('dateOfBirth').value, 'yyyy-MM-dd'));
    formData.append('speciesid',this.addAnimalForm.get('speciesid').value);
    formData.append('photo',this.addAnimalForm.get('photo').value);
    this.animalService.addAnimal(formData).subscribe((animal: AnimalDto) => {this.ngbModal.close(animal)}, err => console.log(err));
  }

  fileChangeEvent(event: any): void {this.imageChangedEvent = event;}
  imageCropped(event: ImageCroppedEvent) {
      let myfile = base64ToFile(event.base64);
      this.addAnimalForm.patchValue({ photo: myfile });
      this.addAnimalForm.get('photo').updateValueAndValidity();
  }
  imageLoaded() {}
  cropperReady() {}
  loadImageFailed() {}

}
