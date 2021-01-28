import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { base64ToFile, ImageCroppedEvent } from 'ngx-image-cropper';
import { AnimalService } from 'src/app/_services/animal.service';

@Component({
  selector: 'app-edit-animal',
  templateUrl: './edit-animal.component.html',
  styleUrls: ['./edit-animal.component.css']
})
export class EditAnimalComponent implements OnInit {
  animal: any;
  active = 1;
  imageChangedEvent: any = '';
  croppedImage: any = '';

  editAnimalForm: FormGroup;
  maxDate: Date;
  speciesList: any = [
    {
      name: 'kutya',
      id: 1
    },
    {
      name: 'macska',
      id: 2
    },
    {
      name: 'nyúl',
      id: 3
    }
    ];
  constructor(private animalService: AnimalService, private route: ActivatedRoute, private fb: FormBuilder, private datepipe: DatePipe) { }

  ngOnInit(): void {
    this.animalService.getAnimal(this.route.snapshot.paramMap.get('animalid')).subscribe( res => {
      this.refreshData();
      this.maxDate = new Date();
    })
  }

  refreshData(){
    this.animalService.getAnimal(this.route.snapshot.paramMap.get('animalid')).subscribe( res => {
      this.animal = res; 
      this.initializeForm();
    })
  }

  initializeForm() {
    this.editAnimalForm = this.fb.group({
      id: [this.animal.id],
      name: [this.animal.name, Validators.required],
      sex: [this.animal.sex, Validators.required],
      dateOfBirth: [this.animal.dateOfBirth, Validators.required],
      speciesid: [this.animal.speciesId, Validators.required],
      weight: [this.animal.weight],
      subspecies: [this.animal.subSpecies],
    })
  }

  updateAnimal(){
    if(this.editAnimalForm.get('weight').value===""){
      this.editAnimalForm.patchValue({ weight: null });
    }
    if(this.editAnimalForm.get('subspecies').value===""){
      this.editAnimalForm.patchValue({ subspecies: null });
    }
    this.animalService.updateAnimal(this.editAnimalForm.value).subscribe(() => {console.log('done'); this.refreshData()}, err => console.log(err));;
  }

  updatePhoto(){
    let formData = new FormData();
    formData.append('id', this.route.snapshot.paramMap.get('animalid'));
    formData.append('photo', this.croppedImage);
    this.animalService.updatePhoto(formData).subscribe(res => {
      console.log(res);
      this.imageChangedEvent = '';
      this.croppedImage = '';
      this.refreshData();
    }, err => console.log(err));
  }

  deletePhoto(){
    this.animalService.deletePhoto(this.route.snapshot.paramMap.get('animalid')).subscribe(res => {
      console.log(res);
      this.refreshData();
    });
  }


  fileChangeEvent(event: any) {this.imageChangedEvent = event;}
  imageCropped(event: ImageCroppedEvent) {this.croppedImage = base64ToFile(event.base64);}
  imageLoaded() {}
  cropperReady() {}
  loadImageFailed() {}

}
