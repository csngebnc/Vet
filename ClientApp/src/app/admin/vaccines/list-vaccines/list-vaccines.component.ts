import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { VaccineDto } from 'src/app/_models/VaccineDto';
import { VaccineService } from 'src/app/_services/vaccine.service';
import { AddVaccineComponent } from '../add-vaccine/add-vaccine.component';
import { EditVaccineComponent } from '../edit-vaccine/edit-vaccine.component';

@Component({
  selector: 'app-list-vaccines',
  templateUrl: './list-vaccines.component.html',
  styleUrls: ['./list-vaccines.component.css']
})
export class ListVaccinesComponent implements OnInit {

  vaccines: VaccineDto[] = [];

  dataSource;
  displayedColumns: string[] = ['id', 'name', 'button'];
  constructor(private vaccineService: VaccineService, private modalService: NgbModal) {
  }

  ngOnInit(): void {
    this.refreshVaccines();
  }

  open() {
    const modalRef = this.modalService.open(AddVaccineComponent);
    modalRef.result.then((vaccine) => {
      this.vaccines.push(vaccine);
      this.dataSource = new MatTableDataSource<VaccineDto>(this.vaccines);
    }, () => { })
  }

  openEdit(id) {
    const modalRef = this.modalService.open(EditVaccineComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then((vaccine) => {
      this.vaccines[this.vaccines.map((item) => {return item.id}).indexOf(vaccine.id)] = vaccine;
      this.dataSource = new MatTableDataSource<VaccineDto>(this.vaccines);
    }, () => { })
  }

  refreshVaccines() {
    this.vaccineService.getVaccines().subscribe((vaccines: VaccineDto[]) => {
      this.vaccines = vaccines;
      this.dataSource = new MatTableDataSource<VaccineDto>(this.vaccines);
    })
  }


  deleteVaccines(id) {
    this.vaccineService.deleteVaccine(id).subscribe(() => { 
      this.vaccines.splice(this.vaccines.map((item) => {return item.id}).indexOf(id), 1) 
      this.dataSource = new MatTableDataSource<VaccineDto>(this.vaccines);
     });
  }

}
