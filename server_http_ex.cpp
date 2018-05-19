
#include <algorithm>
#include <iostream>
#include <sstream>
#include <string>
#include <dirent.h>
#include <dlib/server.h>
#include <dlib/dnn.h>
#include <dlib/gui_widgets.h>
#include <dlib/clustering.h>
#include <dlib/string.h>
#include <dlib/image_io.h>
#include <dlib/image_processing/frontal_face_detector.h>

using namespace dlib;
using namespace std;

template <template <int,template<typename>class,int,typename> class block, int N, template<typename>class BN, typename SUBNET>
using residual = add_prev1<block<N,BN,1,tag1<SUBNET>>>;

template <template <int,template<typename>class,int,typename> class block, int N, template<typename>class BN, typename SUBNET>
using residual_down = add_prev2<avg_pool<2,2,2,2,skip1<tag2<block<N,BN,2,tag1<SUBNET>>>>>>;

template <int N, template <typename> class BN, int stride, typename SUBNET> 
using block  = BN<con<N,3,3,1,1,relu<BN<con<N,3,3,stride,stride,SUBNET>>>>>;

template <int N, typename SUBNET> using ares      = relu<residual<block,N,affine,SUBNET>>;
template <int N, typename SUBNET> using ares_down = relu<residual_down<block,N,affine,SUBNET>>;

template <typename SUBNET> using alevel0 = ares_down<256,SUBNET>;
template <typename SUBNET> using alevel1 = ares<256,ares<256,ares_down<256,SUBNET>>>;
template <typename SUBNET> using alevel2 = ares<128,ares<128,ares_down<128,SUBNET>>>;
template <typename SUBNET> using alevel3 = ares<64,ares<64,ares<64,ares_down<64,SUBNET>>>>;
template <typename SUBNET> using alevel4 = ares<32,ares<32,ares<32,SUBNET>>>;

using anet_type = loss_metric<fc_no_bias<128,avg_pool_everything<
                            alevel0<
                            alevel1<
                            alevel2<
                            alevel3<
                            alevel4<
                            max_pool<3,3,2,2,relu<affine<con<32,7,7,2,2,
                            input_rgb_image_sized<150>
                            >>>>>>>>>>>>;

// ----------------------------------------------------------------------------------------
static const std::string base64_chars = 
             "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
             "abcdefghijklmnopqrstuvwxyz"
             "0123456789+/";


static inline bool is_base64(unsigned char c) {
  return (isalnum(c) || (c == '+') || (c == '/'));
}

std::string base64_encode(unsigned char const* bytes_to_encode, unsigned int in_len) {
  std::string ret;
  int i = 0;
  int j = 0;
  unsigned char char_array_3[3];
  unsigned char char_array_4[4];

  while (in_len--) {
    char_array_3[i++] = *(bytes_to_encode++);
    if (i == 3) {
      char_array_4[0] = (char_array_3[0] & 0xfc) >> 2;
      char_array_4[1] = ((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4);
      char_array_4[2] = ((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6);
      char_array_4[3] = char_array_3[2] & 0x3f;

      for(i = 0; (i <4) ; i++)
        ret += base64_chars[char_array_4[i]];
      i = 0;
    }
  }

  if (i)
  {
    for(j = i; j < 3; j++)
      char_array_3[j] = '\0';

    char_array_4[0] = (char_array_3[0] & 0xfc) >> 2;
    char_array_4[1] = ((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4);
    char_array_4[2] = ((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6);
    char_array_4[3] = char_array_3[2] & 0x3f;

    for (j = 0; (j < i + 1); j++)
      ret += base64_chars[char_array_4[j]];

    while((i++ < 3))
      ret += '=';

  }

  return ret;

}
std::string base64_decode(std::string const& encoded_string) {
  int in_len = encoded_string.size();
  int i = 0;
  int j = 0;
  int in_ = 0;
  unsigned char char_array_4[4], char_array_3[3];
  std::string ret;

  while (in_len-- && ( encoded_string[in_] != '=') && is_base64(encoded_string[in_])) {
    char_array_4[i++] = encoded_string[in_]; in_++;
    if (i ==4) {
      for (i = 0; i <4; i++)
        char_array_4[i] = base64_chars.find(char_array_4[i]);

      char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
      char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
      char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

      for (i = 0; (i < 3); i++)
        ret += char_array_3[i];
      i = 0;
    }
  }

  if (i) {
    for (j = i; j <4; j++)
      char_array_4[j] = 0;

    for (j = 0; j <4; j++)
      char_array_4[j] = base64_chars.find(char_array_4[j]);

    char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
    char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
    char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

    for (j = 0; (j < i - 1); j++) ret += char_array_3[j];
  }

  return ret;
}

std::vector<matrix<rgb_pixel>> faces;
std::vector<string> faceindex;
std::vector<matrix<float,0,1>> face_descriptors;
 frontal_face_detector detector = get_frontal_face_detector();
   shape_predictor sp;
    anet_type net;
int dirinfo()
{
 matrix<rgb_pixel> img1;
       DIR *dp;
    struct dirent *dirp;
    string dirname="/home/finch/dev/face_recognition-master/face_recognition/bbb";

      deserialize("shape_predictor_5_face_landmarks.dat") >> sp;

    deserialize("dlib_face_recognition_resnet_model_v1.dat") >> net;
    if((dp=opendir(dirname.c_str()))==NULL)
    {
        cout << "can't open" << dirname << endl;
    }
      int iii=0;
    while((dirp=readdir(dp))!=NULL)
    {
        cout << dirp->d_name << "--"<< iii++ <<endl;
        string ttt=dirp->d_name;
        std::size_t found=ttt.find(".jpg");
        if(found ==std::string::npos) continue;
        load_image(img1, dirname+"/"+dirp->d_name);
        for (auto face : detector(img1))
        {
            auto shape = sp(img1, face);
            matrix<rgb_pixel> face_chip;
            extract_image_chip(img1, get_face_chip_details(shape,150,0.25), face_chip);
            faces.push_back(move(face_chip));
            faceindex.push_back(dirp->d_name);
        }
    }
    closedir(dp);
     face_descriptors = net(faces);
    return 0;
}

typedef struct simscore
{
    int index;
    float score;
}sscore;

bool sscomp(const sscore &a,const sscore &b){
    return a.score<b.score;
}
string needleinocean(string idfile) try
{
    matrix<rgb_pixel> img1;
    
      std::vector<matrix<rgb_pixel>> facecheck;
    cout << "before" << endl;
        load_image(img1, idfile);
         cout << "after load!" << endl;
        for (auto face : detector(img1))
        {
            auto shape = sp(img1, face);
            matrix<rgb_pixel> face_chip;
            extract_image_chip(img1, get_face_chip_details(shape,150,0.25), face_chip);
            facecheck.push_back(move(face_chip));
        }
  
 
    if (facecheck.size() == 0)
    {
        cout << "No faces found in image!" << endl;
        return "No faces found in image!";
    }

    // This call asks the DNN to convert each face image in faces into a 128D vector.
    // In this 128D vector space, images from the same person will be close to each other
    // but vectors from different people will be far apart.  So we can use these vectors to
    // identify if a pair of images are from the same person or from different people.  
    std::vector<matrix<float,0,1>> face_descriptors_check = net(facecheck);

    // In particular, one simple thing we can do is face clustering.  This next bit of code
    // creates a graph of connected faces and then uses the Chinese whispers graph clustering
    // algorithm to identify how many people there are and which faces belong to whom.
    std::vector<sample_pair> edges;
    std::vector<sscore> score;
    for (size_t i = 0; i < face_descriptors.size(); ++i)
    {
        for (size_t j = 0; j < face_descriptors_check.size(); ++j)
        {            
            float sim=length(face_descriptors[i]-face_descriptors_check[j]);
            if ( sim < 0.6) {
                sscore ss;
                ss.index=i;
                ss.score=sim;
 score.push_back(ss);
            }               
        }
    }
    sort(score.begin(),score.end(),sscomp);
    //char str[20];
    //_gcvt_s(str,sizeof(str),score[0].score,18);

    stringstream ss;
    ss<<score[0].score<< "--"<<score.size() <<flush;

    return faceindex[score[0].index]+ss.str();
}
catch (std::exception& e)
{
    cout << e.what() << endl;
    return "error";
}


class web_server : public server_http
{
    const std::string on_request ( 
        const incoming_things& incoming,
        outgoing_things& outgoing
    )
    {
        ostringstream sout;
       if(incoming.request_type=="POST"){
     sout << "it's post "         << incoming.path << endl;
     if(incoming.path=="/oneofn"){
         ofstream myfile;
         string fname = tmpnam(NULL);
         cout << "filename:" << fname << endl;
         myfile.open(fname);
        // string fc=base64_decode(incoming.body.substr(1,incoming.body.size()-2));
         string fc=base64_decode(incoming.body);
        //  cout << "content:" << fc << endl;
         myfile <<fc;
         myfile.close();
        sout<<fname << "it's post "         << incoming.path << endl;
        sout << needleinocean(fname).c_str() << endl;
        return sout.str();
    }
}
  sout << " <html> <body> "
            << "<form action='/form_handler' method='post'> "
            << "User Name: <input name='user' type='text'><br>  "
            << "User password: <input name='pass' type='text'> <input type='submit'> "
            << " </form>"; 

        // Write out some of the inputs to this request so that they show up on the
        // resulting web page.
        sout << "<br>  path = "         << incoming.path << endl;
        sout << "<br>  request_type = " << incoming.request_type << endl;
        sout << "<br>  content_type = " << incoming.content_type << endl;
        sout << "<br>  protocol = "     << incoming.protocol << endl;
        sout << "<br>  foreign_ip = "   << incoming.foreign_ip << endl;
        sout << "<br>  foreign_port = " << incoming.foreign_port << endl;
        sout << "<br>  local_ip = "     << incoming.local_ip << endl;
        sout << "<br>  local_port = "   << incoming.local_port << endl;
        sout << "<br>  body = \""       << incoming.body << "\"" << endl;

      //  
        // If this request is the result of the user submitting the form then echo back
        // the submission.
        if (incoming.path == "/form_handler")
        {
            sout << "<h2> Stuff from the query string </h2>" << endl;
            sout << "<br>  user = " << incoming.queries["user"] << endl;
            sout << "<br>  pass = " << incoming.queries["pass"] << endl;

            // save these form submissions as cookies.  
            outgoing.cookies["user"] = incoming.queries["user"];
            outgoing.cookies["pass"] = incoming.queries["pass"];
        }


        // Echo any cookies back to the client browser 
        sout << "<h2>Cookies the web browser sent to the server</h2>";
        for ( key_value_map::const_iterator ci = incoming.cookies.begin(); ci != incoming.cookies.end(); ++ci )
        {
            sout << "<br/>" << ci->first << " = " << ci->second << endl;
        }

        sout << "<br/><br/>";

        sout << "<h2>HTTP Headers the web browser sent to the server</h2>";
        // Echo out all the HTTP headers we received from the client web browser
        for ( key_value_map_ci::const_iterator ci = incoming.headers.begin(); ci != incoming.headers.end(); ++ci )
        {
            sout << "<br/>" << ci->first << ": " << ci->second << endl;
        }

        sout << "</body> </html>";

        return sout.str();
    }

};

int main(int argc,char* argv[])
{
    try
    {
         cout << "needleinocean(argv[1]).c_str()" << endl;
        dirinfo();
         cout << "adsjflkajdf" << endl;
      //  cout << needleinocean(argv[1]).c_str() << endl;
        web_server our_web_server;

        // make it listen on port 5000
        our_web_server.set_listening_port(5000);
        // Tell the server to begin accepting connections.
        our_web_server.start_async();

        cout << "Press enter to end this program" << endl;
        cin.get();
    }
    catch (exception& e)
    {
        cout << e.what() << endl;
    }
}




