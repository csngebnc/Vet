import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { AnimalDto } from 'src/app/_models/animaldto';
import { MedicalRecordDto } from 'src/app/_models/MedicalRecordDto';
import { AnimalService } from 'src/app/_services/animal.service';
import { MedicalRecordService } from 'src/app/_services/medical-record.service';
import { ListVaccineRecordsComponent } from '../../vaccine-records/list-vaccine-records/list-vaccine-records.component';
import { EnlargeImageModalComponent } from '../enlarge-image-modal/enlarge-image-modal.component';

@Component({
  selector: 'app-list-records',
  templateUrl: './list-records.component.html',
  styleUrls: ['./list-records.component.css']
})
export class ListRecordsComponent implements OnInit {

  viewLevel: number;

  constructor(protected auth: AuthorizeService,
    private animalService: AnimalService,
    private recordService: MedicalRecordService,
    private route: ActivatedRoute,
    private modalService: NgbModal) { }

  animal: AnimalDto;
  meds: MedicalRecordDto[] = [];

  ngOnInit(): void {
    const animalId = this.route.snapshot.paramMap.get('animalid');
    this.animalService.getAnimal(animalId).subscribe((animal: AnimalDto) => {
      this.animal = animal;
      this.recordService.getMedicalRecordByAnimalId(animalId).subscribe((records: MedicalRecordDto[]) => {
        this.viewLevel = this.auth.authLevel;
        this.meds = records;
      });
    });
  }

  log() {
    console.log('helolo');
  }


  openBig(path) {
    const modalRef = this.modalService.open(EnlargeImageModalComponent, { size: 'lg' });
    modalRef.componentInstance.imagePath = path;
    modalRef.result.then(() => { }, () => { })
  }

  openVaccineRecords() {
    const modalRef = this.modalService.open(ListVaccineRecordsComponent, { size: 'lg' });
    modalRef.componentInstance.animalId = this.animal.id;
    modalRef.result.then(() => { }, () => { })
  }

}
