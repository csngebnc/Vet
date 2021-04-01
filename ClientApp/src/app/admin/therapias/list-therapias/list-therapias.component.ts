import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddSpeciesComponent } from 'src/app/species/add-species/add-species.component';
import { EditSpeciesComponent } from 'src/app/species/edit-species/edit-species.component';
import { TherapiaDto } from 'src/app/_models/therapiadto';
import { TherapiaService } from 'src/app/_services/therapia.service';
import { AddTherapiaComponent } from '../add-therapia/add-therapia.component';
import { EditTherapiaComponent } from '../edit-therapia/edit-therapia.component';

@Component({
  selector: 'app-list-therapias',
  templateUrl: './list-therapias.component.html',
  styleUrls: ['./list-therapias.component.css']
})
export class ListTherapiasComponent implements OnInit {

  therapias: TherapiaDto[] = [];

  dataSource;
  displayedColumns: string[] = ['id', 'name', 'priceperunit', 'status', 'button'];

  constructor(private therapiaService: TherapiaService, private modalService: NgbModal) { this.refreshTherapias(); }

  ngOnInit(): void {
  }

  open() {
    const modalRef = this.modalService.open(AddTherapiaComponent);
    modalRef.result.then(() => this.refreshTherapias(), () => { })
  }

  openEdit(id) {
    const modalRef = this.modalService.open(EditTherapiaComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then(() => this.refreshTherapias(), () => { })
  }

  refreshTherapias() {
    this.therapiaService.getTherapias().subscribe((therapias: TherapiaDto[]) => {
      this.therapias = therapias;
      this.dataSource = new MatTableDataSource<TherapiaDto>(this.therapias);
    })
  }

  changeStateOfSpecies(id) {
    this.therapiaService.changeState(id).subscribe(() => {
      this.refreshTherapias();
    })
  }

  deleteSpecies(id) {
    this.therapiaService.deleteTherapia(id).subscribe(() => { this.refreshTherapias() });
  }


}
