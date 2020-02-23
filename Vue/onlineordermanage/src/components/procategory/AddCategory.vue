<template>
  <el-form ref="form" :model="form" label-width="80px">
    <el-form-item label="父级分类">
      <el-select v-model="form.procatevalue" placeholder="请选择">
        <el-option
          v-for="item in form.procateoptions"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        ></el-option>
      </el-select>
    </el-form-item>
    <el-form-item label="分类名称">
      <el-input v-model="form.procatename"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="onSubmit" >添加</el-button>     
      <el-button>取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import http from "../../common/http/vueresource.ts";
//使用http工具
Vue.use(http);
export default {
  data() {
    return {
      form: {
        procateoptions: [
          {
            value: "选项1",
            label: "黄金糕"
          },
          {
            value: "选项2",
            label: "双皮奶"
          },
          {
            value: "选项3",
            label: "蚵仔煎"
          },
          {
            value: "选项4",
            label: "龙须面"
          },
          {
            value: "选项5",
            label: "北京烤鸭"
          }
        ],
        procatename: ""
      }
    };
  },
  mounted:function(){

  },
  methods:{
      initData(){
          
      },
      onSubmit(){

      }
  }
};
</script>