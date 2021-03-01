import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TreatmentTimeDto } from 'src/app/_models/treatmenttimedto';
import { TreatmenttimeService } from 'src/app/_services/treatmenttime.service';
import { AddTreatmentTimeComponent } from '../add-treatment-time/add-treatment-time.component';
import { EditTreatmentTimeComponent } from '../edit-treatment-time/edit-treatment-time.component';

@Component({
  selector: 'app-list-treatment-time',
  templateUrl: './list-treatment-time.component.html',
  styleUrls: ['./list-treatment-time.component.css']
})
export class ListTreatmentTimeComponent implements OnInit {

  days: string[] = ["vasárnap", "hétfő", "kedd", "szerda", "csütörtök", "péntek", "szombat"];

  treatmentTimes: TreatmentTimeDto[] = [];
  constructor(private treatmentTimeService: TreatmenttimeService, private route: ActivatedRoute, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.refreshTimes();
  }

  refreshTimes(){
    this.treatmentTimeService.getTreatmentTimeByTreatmentId(this.route.snapshot.paramMap.get('treatmentid'))
      .subscribe((treatmentTimes: TreatmentTimeDto[]) => {
        this.treatmentTimes = treatmentTimes;
        console.log(this.route.snapshot.paramMap.get('treatmentid'));
        console.log(this.treatmentTimes);
      })
  }

  open(){
    const modalRef = this.modalService.open(AddTreatmentTimeComponent, {size: 'lg'});
    modalRef.componentInstance.treatmentId = this.route.snapshot.paramMap.get('treatmentid');
    modalRef.result.then(() => {this.refreshTimes();},() => {})
  }

  openEdit(id){
    const modalRef = this.modalService.open(EditTreatmentTimeComponent, {size: 'lg'});
    modalRef.componentInstance.id = id;
    modalRef.result.then(() => this.refreshTimes(), () => {});
  }

  deleteTreatmentTime(id){
    this.treatmentTimeService.deleteTreatmentTime(id).subscribe(() => this.refreshTimes());
  }

  changeStateOfTreatmentTime(id){
    this.treatmentTimeService.changeState(id).subscribe(()=> this.refreshTimes());
  }
}
