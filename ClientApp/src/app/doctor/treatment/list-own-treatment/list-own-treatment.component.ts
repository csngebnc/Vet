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
    this.treatmentService.getOwnTreatments().subscribe((treatments: TreatmentDto[]) => {
      this.treatments = treatments;
      this.dataSource = new MatTableDataSource<TreatmentDto>(this.treatments);
    })
  }

  open() {
    const modalRef = this.modalService.open(AddTreatmentComponent);
    modalRef.result.then((treatment: TreatmentDto) => {
      this.treatments.push(treatment);
      this.dataSource = new MatTableDataSource<TreatmentDto>(this.treatments);
    }, () => { })
  }

  openEdit(id) {
    const modalRef = this.modalService.open(EditTreatmentComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then((treatment) => {
      this.treatments[this.treatments.map((t) => { return t.id }).indexOf(id)] = treatment;
      this.dataSource = new MatTableDataSource<TreatmentDto>(this.treatments);
    }, () => { })
  }

  deleteTreatment(id: number) {
    this.treatmentService.deleteTreatment(id).subscribe(() => {
      this.treatments = this.treatments.filter(t => t.id !== id);
      this.dataSource = new MatTableDataSource<TreatmentDto>(this.treatments);
    })
  }

  changeStateOfTreatment(id) {
    this.treatmentService.changeState(id).subscribe(() => {
      this.treatments[this.treatments.map(t => t.id).indexOf(id)].isInactive = !this.treatments[this.treatments.map(t => t.id).indexOf(id)].isInactive;
      this.dataSource = new MatTableDataSource<TreatmentDto>(this.treatments);

    })
  }



}
