(function($){
  'use strict';
  // write code here and click the execute button
  // const App = (props) => {
  //  	return (
  //   	<div>
  //       {props.cards.map( card => <Card name={card.name} question={card.question} answer={card.answer}/>)}
  //     </div>
  //   )
  // }


 // ReactDOM.render(<App cards={data}/>, document.getElementById('container'));

  // 'use strict';
  // // write code here and click the execute button
  // const App = (props) => {
  //   return (
  //     <div>
  //       {props.games.map( game => <Game img={game.img} name={game.name} tag={game.tag}/>)}
  //     </div>
  //   )
  // }

  // const Game = (props) => {
  //   return (
  //     <div className="game">
  //       <img src={props.img} />
  //         <span>
  //           <div className="gamename">{props.name} </div>
  //           <div className="tags">{props.tags}</div>
  //         </span>
  //     </div>
  //   )
  // }

  // const data = [
  // { name: "This War of Mine",
  //   id: "282070",
  //   img:"http://cdn.akamai.steamstatic.com/steamcommunity/public/images/apps/282070/4d2ecaa400d7937fdfae6642b73ad5695c393bf7.jpg",
  //   desc: "In This War Of Mine you do not play as an elite soldier, rather a group of civilians trying to survive in a besieged city; struggling with lack of food, medicine and constant danger from snipers and hostile scavengers. The game provides an experience of war seen from an entirely new angle.",
  //   tags: ["war", "rpg", "violent"]},

  // {"id":"268750",
  //   "name":"Magicite",
  //   "img":"http://cdn.akamai.steamstatic.com/steamcommunity/public/images/apps/268750/fa19f06a256bfba8ce6084e0dcdf18ce7f2ab766.jpg",
  //   "desc":"Explore, craft, and survive in this Multiplayer RPG Platformer with permanent death! Featuring many Rogue-like elements, Magicite randomly generates each underground dungeon for you and your friends to delve deep into.",
  //   tags: ["fantasy", "action", "violent"]}
  // ]

  // ReactDOM.render(<App games={data} />, document.getElementById('container'));

  var steps = 1000;

  checkitem();
  renderSteps();
  $('#text-carousel').bind('slid.bs.carousel', function (e) {
      checkitem();
  });

  document.querySelector("#add-card").addEventListener("click", function(){

  $(".item").removeClass('active');

  $(".carousel-inner")
    .append('<div class="item active"> <div class="carousel-content"> <div class="card"> <div class="card-header"> Steps: <span class="step-count"></span> </div> <div class="card-block"> <h4 class="card-title">___NEW!!!___RIGHTMOST Special title treatment</h4> <p class="card-text">RIGHTMOST With supporting text below as a natural lead-in to additional content.</p> <a href="#" class="btn btn-primary">Go somewhere</a> </div> </div> </div> </div>');
  });


  function checkitem(){
    var $this = $('#text-carousel');
    if($('.carousel-inner .item:first').hasClass('active')) {
      $this.children('.left.carousel-control').hide();
      $this.children('.right.carousel-control').show();
    } else if($('.carousel-inner .item:last').hasClass('active')) {
      $this.children('.left.carousel-control').show();
      $this.children('.right.carousel-control').hide();
    } else {
      $this.children('.carousel-control').show();

    } 
  }

  function renderSteps(){
    $(".step-count").html(steps)
  }


})(jQuery);
