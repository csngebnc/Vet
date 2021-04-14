import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AnimalDto } from 'src/app/_models/animaldto';
import { AnimalService } from 'src/app/_services/animal.service';

@Component({
  selector: 'app-list-archived-animal-by-owner',
  templateUrl: './list-archived-animal-by-owner.component.html',
  styleUrls: ['./list-archived-animal-by-owner.component.css']
})
export class ListArchivedAnimalByOwnerComponent implements OnInit {


  animals: AnimalDto[] = [];
  page = 1;
  pageSize = 8;

  constructor(private animalService: AnimalService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.animalService.getArchivedAnimalsByUserId(this.route.snapshot.paramMap.get('userid')).subscribe((animals: AnimalDto[]) => {
      this.animals = animals;
    })
  }

  deleteAnimal(id: number) {
    if (confirm('Biztosan szeretnéd törölni?')) {
      this.animalService.deleteAnimal(id).subscribe(() => {
        let deleteIdx = this.animals.map((a) => { return a.id }).indexOf(id);
        this.animals.splice(deleteIdx, 1);
      });
    }
  }

  archiveAnimal(id) {
    if (confirm('Biztosan szeretnéd visszaállítani?')) {
      this.animalService.changeStateOfAnimal(id).subscribe(() => {
        let deleteIdx = this.animals.map((a) => { return a.id }).indexOf(id);
        this.animals.splice(deleteIdx, 1);
      });
    }
  }

}
