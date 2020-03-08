<template>
  <div id="app">
    <!-- <MenuList /> -->
    <CategoryList />
  </div>
</template>

<script>
// import HelloWorld from './components/HelloWorld.vue'
// import AddPro from "./components/product/AddPro.vue";
// import AddCategory from "./components/procategory/AddCategory.vue";
import CategoryList from "./components/procategory/CategoryList.vue";
//import MenuList from "./components/menu/munulist.vue";

export default {
  name: "App",
  components: {
    // HelloWorld,
    //MenuList,
    CategoryList
  }
};
</script>

<style>
</style>
