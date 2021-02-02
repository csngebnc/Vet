import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { base64ToFile, ImageCroppedEvent } from 'ngx-image-cropper';
import { AnimalDto } from 'src/app/_models/animaldto';
import { AnimalService } from 'src/app/_services/animal.service';
import { SpeciesService } from 'src/app/_services/species.service';

@Component({
  selector: 'app-edit-animal',
  templateUrl: './edit-animal.component.html',
  styleUrls: ['./edit-animal.component.css']
})
export class EditAnimalComponent implements OnInit {
  animal: AnimalDto;
  active = 1;
  imageChangedEvent: any = '';
  croppedImage: any = '';

  editAnimalForm: FormGroup;
  maxDate: Date;
  speciesList: any;
  constructor(private animalService: AnimalService, private speciesService: SpeciesService, private route: ActivatedRoute, private fb: FormBuilder, private datepipe: DatePipe) { 
    this.refreshData();
  }

  ngOnInit(): void {
    this.maxDate = new Date();
  }

  refreshData(){
    this.animalService.getAnimal(this.route.snapshot.paramMap.get('animalid')).subscribe( (animal: AnimalDto) => {
      this.animal = animal;
      this.speciesService.getAnimalSpecies().subscribe(response => {
        this.speciesList = response;
        this.initializeForm();
      }) 
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
    this.animalService.updateAnimal(this.editAnimalForm.value).subscribe(() => {this.refreshData()}, err => console.log(err));;
  }

  updatePhoto(){
    let formData = new FormData();
    formData.append('id', this.route.snapshot.paramMap.get('animalid'));
    formData.append('photo', this.croppedImage);

    this.animalService.updatePhoto(formData).subscribe(res => {
      this.imageChangedEvent = '';
      this.croppedImage = '';
      this.refreshData();
    }, err => console.log(err));
  }

  deletePhoto(){
    this.animalService.deletePhoto(this.route.snapshot.paramMap.get('animalid')).subscribe(() => {
      this.refreshData();
    });
  }


  fileChangeEvent(event: any) {this.imageChangedEvent = event;}
  imageCropped(event: ImageCroppedEvent) {this.croppedImage = base64ToFile(event.base64);}
  imageLoaded() {}
  cropperReady() {}
  loadImageFailed() {}

}
