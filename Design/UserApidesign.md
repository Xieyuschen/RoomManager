# 一期api设计  
此设计仅提供方向，细节自行决定  
  


## 用户设计  
用户的身份认证使用rolebase原则，设置三个基础role：Anonymous, User, Admin  
注意role的设计思路：并不是只要拥有admin身份就能干user能干的事，admin能干user能干的事情还必须有user role  
也就是说一个用户应当可具有多个role，这些role是他的身份。  
```cpp
int factorial(int num){
    int result=1;
    while(num){
        result*=num;
        num--;
    }
    return result;
}
int main(){
    int res;
    scanf("%d",&num);
    res=factorial(num);
    printf("%d",res);
    return 0;
};


```
### api/user/login  
全站除login外全部要求身份认证。login不要求身份认证。  
http谓词：post  
传参方式：表单  
dto：  
```json  
{
    email:"string",
    password:"string",
    rememberMe:true
}
```
返回信息dto:
```json  
{
    id:"123123",
    email:"a@a.com",
    roles:["User"],
    avatarUrl:"/ads/asd/as.jpg",
    Name:"Bill"
}
```
前端在任何页面初始化时调用login(此时传空参数)  
后端自动判断，若表单信息为空则分两种情况：  
1. 已登录：自动返回已登录用户的信息。
2. 没登陆：以Anonymous身份登陆，返回该身份的信息。

若不为空，尝试用该信息登陆。若失败，返回400.  
  

### api/user/logout  
过期用户的cookie，自动重新登录为Anonymous,返回Anonymous信息
* url: \user\logout
* verb：post
* 没有输入的参数
* response: 
    * 200:
    ```json
    {
        id:"123123",
        email:"a@a.com",
        roles:["User"],
        avatarUrl:"/ads/asd/as.jpg",
        Name:"Bill"
    }
    ```


### api/user/{id?}
* url: /user/{id?}
* verb: get
* parameter: id,用户id，如果不加此参数则返回当前登录用户的信息，匿名用户访问自己给403
* response:
    * 200:
    * response:
    * 200:
    ```json
    {
        id:"111",
        name:"Bill",
        affiliation:"HUST",
        keywords:["数学","核聚变"],
        biography:"我是一个伟大的科学家",
        education:["附小","附中","HUST"],
        roles:["User"],
        isSelf:true,
        avatarUrl:"asdasdasd.jpg",
        email:"a@a.com",
    }
    ```
### api/user/newuser
新建一个用户 
* url: \user\newUser
* verb: post  
* parameter：
    * body:
    ```json
    {
        email:"a@a.com",
        name:"Bill",
        password:"qweqwe123",
    }
    ```
* response:
    * 200:
    ```json //同login logout
    {
        id:123123,
        email:"a@a.com",
        roles:["User"],
        avatarUrl:"/ads/asd/as.jpg",
        Name:"Bill"
    }
    ```
    Header:set user cookie to the login user
    * 400:
    ```json
    {
        errorMessages:["duplicate users","Some problems"]
    }
    ```

### api/user
1. 更改用户资料
2. email暂时不让改
* url: \user
* verb: post
* parameter:
    ```json
    {
        id:111,
        name:"Bill",
        affiliation:"HUST",
        keywords:["数学","核聚变"],
        biography:"我是一个伟大的科学家",
        education:["附小","附中","HUST"]
    }
    ```
* response:
    * 200:
    ```{id:111}```

