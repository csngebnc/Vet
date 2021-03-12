import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TreatmentDto } from 'src/app/_models/treatmentdto';
import { TreatmentService } from 'src/app/_services/treatment.service';
import { AddTreatmentComponent } from '../add-treatment/add-treatment.component';
import { EditTreatmentComponent } from '../edit-treatment/edit-treatment.component';

@Component({
  selector: 'app-list-own-treatment',
  templateUrl: './list-own-treatment.component.html',
  styleUrls: ['./list-own-treatment.component.css']
})
export class ListOwnTreatmentComponent implements OnInit {

  treatments: TreatmentDto[] = [];
  dataSource;
  displayedColumns: string[] = ['id', 'name', 'status', 'button'];

  constructor(private treatmentService: TreatmentService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.refreshTreatments();
  }

  refreshTreatments() {
    this.treatmentService.getOwnTreatments().subscribe((treatments: TreatmentDto[]) => {
      this.treatments = treatments;
      this.dataSource = new MatTableDataSource<TreatmentDto>(this.treatments);
    })
  }

  open() {
    const modalRef = this.modalService.open(AddTreatmentComponent);
    modalRef.result.then(() => this.refreshTreatments())
  }

  openEdit(id) {
    const modalRef = this.modalService.open(EditTreatmentComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then(() => this.refreshTreatments())
  }

  deleteTreatment(id) {
    this.treatmentService.deleteTreatment(id).subscribe(() => {
      this.refreshTreatments();
    })
  }

  changeStateOfTreatment(id) {
    this.treatmentService.changeState(id).subscribe(() => {
      this.refreshTreatments();
    })
  }



}
